using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.JSInterop;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using System.Diagnostics;

namespace KeePassLib
{
    public sealed class KPCLibLogger : IStatusLogger
    {
        private bool m_bStartedLogging = false;
        private bool m_bEndedLogging = false;

        private List<KeyValuePair<LogStatusType, string>> m_vCachedMessages =
            new List<KeyValuePair<LogStatusType, string>>();

        public KPCLibLogger()
        {
        }

        ~KPCLibLogger()
        {
            // Debug.Assert(m_bEndedLogging);

            try { if (!m_bEndedLogging) EndLogging(); }
            catch (Exception) { Debug.Assert(false); }
        }

        public void StartLogging(string strOperation, bool bWriteOperationToLog)
        {
            Debug.Assert(!m_bStartedLogging && !m_bEndedLogging);
            if (strOperation != null)
            {
                Debug.WriteLine(strOperation);
            }

            m_bStartedLogging = true;

            if (bWriteOperationToLog)
                m_vCachedMessages.Add(new KeyValuePair<LogStatusType, string>(
                    LogStatusType.Info, strOperation));
        }

        public void EndLogging()
        {
            // Debug.Assert(m_bStartedLogging && !m_bEndedLogging);

            m_bEndedLogging = true;
        }

        /// <summary>
        /// Set the current progress in percent.
        /// </summary>
        /// <param name="uPercent">Percent of work finished.</param>
        /// <returns>Returns <c>true</c> if the caller should continue
        /// the current work.</returns>
        public bool SetProgress(uint uPercent)
        {
            // Debug.Assert(m_bStartedLogging && !m_bEndedLogging);

            bool b = true;

            return b;
        }

        /// <summary>
        /// Set the current status text.
        /// </summary>
        /// <param name="strNewText">Status text.</param>
        /// <param name="lsType">Type of the message.</param>
        /// <returns>Returns <c>true</c> if the caller should continue
        /// the current work.</returns>
        public bool SetText(string strNewText, LogStatusType lsType)
        {
            // Debug.Assert(m_bStartedLogging && !m_bEndedLogging);

            m_vCachedMessages.Clear();

            bool b = true;
            m_vCachedMessages.Add(new KeyValuePair<LogStatusType, string>(
                lsType, strNewText));

            return b;
        }

        /// <summary>
        /// Check if the user cancelled the current work.
        /// </summary>
        /// <returns>Returns <c>true</c> if the caller should continue
        /// the current work.</returns>
        public bool ContinueWork()
        {
            // Debug.Assert(m_bStartedLogging && !m_bEndedLogging);

            bool b = true;

            return b;
        }
    }

    public class Database
    {
        [JSInvokable]
        public static bool KeePassOpen(string path)
        {
            var Logger = new KPCLibLogger();
            var PwDb = new PwDatabase();
            IOConnectionInfo ioc = IOConnectionInfo.FromPath(path);
            CompositeKey cmpKey = new CompositeKey();
            cmpKey.AddUserKey(new KcpPassword("12345"));
            PwDb.Open(ioc, cmpKey, Logger);
            return PwDb.IsOpen;
        }
    }
}
