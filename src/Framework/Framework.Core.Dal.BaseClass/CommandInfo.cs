using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;

namespace Framework.Core.Dal.BaseClass
{
    

    public class CommandInfo
    {
        private EventHandler _solicitationEvent;
        public string CommandText;
        public EffentNextType EffentNextType;
        public object OriginalData;
        public DbParameter[] Parameters;
        public object ShareObject;

        private event EventHandler _solicitationEvent1
        {
            add
            {
                EventHandler handler2;
                EventHandler handler = this._solicitationEvent;
                do
                {
                    handler2 = handler;
                    EventHandler handler3 = (EventHandler) Delegate.Combine(handler2, value);
                    handler = Interlocked.CompareExchange<EventHandler>(ref this._solicitationEvent, handler3, handler2);
                }
                while (handler != handler2);
            }
            remove
            {
                EventHandler handler2;
                EventHandler handler = this._solicitationEvent;
                do
                {
                    handler2 = handler;
                    EventHandler handler3 = (EventHandler) Delegate.Remove(handler2, value);
                    handler = Interlocked.CompareExchange<EventHandler>(ref this._solicitationEvent, handler3, handler2);
                }
                while (handler != handler2);
            }
        }

        public event EventHandler SolicitationEvent
        {
            add
            {
                this._solicitationEvent += value;
            }
            remove
            {
                this._solicitationEvent -= value;
            }
        }

        public CommandInfo()
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = EffentNextType.None;
        }

        public CommandInfo(string sqlText, SqlParameter[] para)
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = EffentNextType.None;
            this.CommandText = sqlText;
            this.Parameters = para;
        }

        public CommandInfo(string sqlText, SqlParameter[] para, EffentNextType type)
        {
            this.ShareObject = null;
            this.OriginalData = null;
            this.EffentNextType = EffentNextType.None;
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }

        public void OnSolicitationEvent()
        {
            if (this._solicitationEvent != null)
            {
                this._solicitationEvent(this, new EventArgs());
            }
        }
    }
}

