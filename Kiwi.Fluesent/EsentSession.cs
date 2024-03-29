using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentSession : IEsentSession
    {
        public IEsentDatabase Database { get; set; }
        private readonly Session _session;
        private readonly IWriteLockable _writeLockable;
        private IDisposable _writeLock;

        public EsentSession(IEsentDatabase database, Session session, IWriteLockable writeLockable)
        {
            Database = database;
            _session = session;
            _writeLockable = writeLockable;
            JetDbid = JET_DBID.Nil;
        }

        #region IEsentSession Members

        public JET_DBID JetDbid { get; protected set; }

        public JET_SESID JetSesid
        {
            get { return _session.JetSesid; }
        }

        public void CreateDatabase(string connect, CreateDatabaseGrbit grbit)
        {
            if (JetDbid != JET_DBID.Nil)
            {
                throw new ApplicationException("Only one database may be opened in an EsentSession");
            }
            JET_DBID dbid;
            Api.JetCreateDatabase(_session, Database.DatabasePath, connect, out dbid, grbit);
            JetDbid = dbid;
        }

        public void OpenDatabase(string connect, OpenDatabaseGrbit grbit)
        {
            if (JetDbid != JET_DBID.Nil)
            {
                throw new ApplicationException("Only one database may be opened in an EsentSession");
            }
            JET_DBID dbid;
            Api.JetOpenDatabase(_session, Database.DatabasePath, connect, out dbid, grbit);
            JetDbid = dbid;
        }

        public void AttachDatabase(AttachDatabaseGrbit grbit)
        {
            Api.JetAttachDatabase(_session, Database.DatabasePath, grbit);
        }

        public void LockWrites()
        {
            if (_writeLock == null)
            {
                _writeLock = _writeLockable.CreateWriteLock();
            }
        }

        public IEsentTransaction CreateTransaction()
        {
            return new EsentTransaction(this, new Transaction(_session));
        }

        public void Dispose()
        {
            if (_session != null)
            {
                _session.Dispose();
            }
            if (_writeLock != null)
            {
                _writeLock.Dispose();
            }
            if (_writeLockable != null)
            {
                _writeLockable.Dispose();
            }
        }

        #endregion
    }
}