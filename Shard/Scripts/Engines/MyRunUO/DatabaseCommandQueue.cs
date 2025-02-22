/*
 *	This program is the CONFIDENTIAL and PROPRIETARY property 
 *	of Tomasello Software LLC. Any unauthorized use, reproduction or
 *	transfer of this computer program is strictly prohibited.
 *
 *      Copyright (c) 2004 Tomasello Software LLC.
 *	This is an unpublished work, and is subject to limited distribution and
 *	restricted disclosure only. ALL RIGHTS RESERVED.
 *
 *			RESTRICTED RIGHTS LEGEND
 *	Use, duplication, or disclosure by the Government is subject to
 *	restrictions set forth in subparagraph (c)(1)(ii) of the Rights in
 * 	Technical Data and Computer Software clause at DFARS 252.227-7013.
 *
 *	Angel Island UO Shard	Version 1.0
 *			Release A
 *			March 25, 2004
 */
/* Scripts/Engines/MyRunUO/DatabaseCommandQueue.cs
 * Changelog
 *  04/28/05 Taran Kain
 *		Modified some SQL commands to avoid exceptions when using transactions
 *		Added in ability to display SQL statements when debugging
 */
using System;
using System.Threading;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using Server.Commands;

namespace Server.Engines.MyRunUO
{
	public class DatabaseCommandQueue
	{
		private Queue m_Queue;
		private ManualResetEvent m_Sync;
		private Thread m_Thread;

		private bool m_HasCompleted;

		private string m_CompletionString;
		private string m_ConnectionString;

		public bool HasCompleted
		{
			get { return m_HasCompleted; }
		}

		public void Enqueue(object obj)
		{
			lock (m_Queue.SyncRoot)
			{
				m_Queue.Enqueue(obj);
				try { m_Sync.Set(); }
				catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
			}
		}

		public DatabaseCommandQueue(string completionString, string threadName)
			: this(Config.CompileConnectionString(), completionString, threadName)
		{
		}

		public DatabaseCommandQueue(string connectionString, string completionString, string threadName)
		{
			m_CompletionString = completionString;
			m_ConnectionString = connectionString;

			m_Queue = Queue.Synchronized(new Queue());

			m_Queue.Enqueue(null); // signal connect

			/*m_Queue.Enqueue("DELETE FROM myrunuo_characters");
			m_Queue.Enqueue("DELETE FROM myrunuo_characters_layers");
			m_Queue.Enqueue("DELETE FROM myrunuo_characters_skills");
			m_Queue.Enqueue("DELETE FROM myrunuo_guilds");
			m_Queue.Enqueue("DELETE FROM myrunuo_guilds_wars");*/

			m_Sync = new ManualResetEvent(true);

			m_Thread = new Thread(new ThreadStart(Thread_Start));
			m_Thread.Name = threadName;//"MyRunUO Database Command Queue";
			m_Thread.Priority = Config.DatabaseThreadPriority;
			m_Thread.Start();
		}

		private void Thread_Start()
		{
			bool connected = false;

			OdbcConnection connection = null;
			OdbcCommand command = null;
			OdbcTransaction transact = null;

			DateTime start = DateTime.Now;

			bool shouldWriteException = true;

			while (true)
			{
				m_Sync.WaitOne();

				while (m_Queue.Count > 0)
				{
					try
					{
						object obj = m_Queue.Dequeue();

						if (obj == null)
						{
							if (connected)
							{
								if (transact != null)
								{
									try
									{
										if (Config.DisplaySQL)
											Console.WriteLine("MyRunUO: Committing Transaction");
										transact.Commit();
									}
									catch (Exception commitException)
									{
										LogHelper.LogException(commitException);
										Console.WriteLine("MyRunUO: Exception caught when committing transaction");
										Console.WriteLine(commitException);

										try
										{
											if (Config.DisplaySQL)
												Console.WriteLine("MyRunUO: Rolling Back Transaction");
											transact.Rollback();
											Console.WriteLine("MyRunUO: Transaction has been rolled back");
										}
										catch (Exception rollbackException)
										{
											LogHelper.LogException(rollbackException);
											Console.WriteLine("MyRunUO: Exception caught when rolling back transaction");
											Console.WriteLine(rollbackException);
										}
									}
								}

								try { connection.Close(); }
								catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

								try { connection.Dispose(); }
								catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

								try { command.Dispose(); }
								catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

								try { m_Sync.Close(); }
								catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

								Console.WriteLine(m_CompletionString, (DateTime.Now - start).TotalSeconds);
								m_HasCompleted = true;

								return;
							}
							else
							{
								try
								{
									connected = true;
									connection = new OdbcConnection(m_ConnectionString);
									connection.Open();
									command = connection.CreateCommand();

									if (Config.UseTransactions)
									{
										if (Config.DisplaySQL)
											Console.WriteLine("MyRunUO: Beginning Transaction");
										transact = connection.BeginTransaction();
										command.Transaction = transact;
									}
								}
								catch (Exception e)
								{
									LogHelper.LogException(e);
									try
									{
										if (transact != null)
										{
											if (Config.DisplaySQL)
												Console.WriteLine("MyRunUO: Rolling Back Transaction");
											transact.Rollback();
										}
									}
									catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

									try { if (connection != null) connection.Close(); }
									catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

									try { if (connection != null) connection.Dispose(); }
									catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

									try { if (command != null) command.Dispose(); }
									catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

									try { m_Sync.Close(); }
									catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

									Console.WriteLine("MyRunUO: Unable to connect to the database");
									Console.WriteLine(e);
									m_HasCompleted = true;
									return;
								}
							}
						}
						else if (obj is string)
						{
							command.CommandText = (string)obj;
							if (Config.DisplaySQL)
								Console.WriteLine("MyRunUO: {0}", command.CommandText);
							command.ExecuteNonQuery();
						}
						else
						{
							string[] parms = (string[])obj;

							command.CommandText = parms[0];
							if (Config.DisplaySQL)
								Console.WriteLine("MyRunUO: {0}", command.CommandText);

							if (command.ExecuteScalar() == null)
							{
								command.CommandText = parms[1];
								if (Config.DisplaySQL)
									Console.WriteLine("MyRunUO: {0}", command.CommandText);
								command.ExecuteNonQuery();
							}
						}
					}
					catch (Exception e)
					{
						LogHelper.LogException(e);
						if (shouldWriteException)
						{
							Console.WriteLine("MyRunUO: Exception caught in database thread");
							Console.WriteLine(e);
							shouldWriteException = false;
						}
					}
				}

				lock (m_Queue.SyncRoot)
				{
					if (m_Queue.Count == 0)
						m_Sync.Reset();
				}
			}
		}
	}
}