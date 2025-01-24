using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal abstract class Transaction
    {
        private string _transactionId;
        private DateTime _transactionDate;
        private Member _member;

        protected Transaction()
        {
            _transactionId = string.Empty;
            _transactionDate = DateTime.Now;
            _member = null!;
        }

        public string TransactionId
        {
            get { return _transactionId; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("TransactionId cannot be empty or null");
                }
                _transactionId = value;
            }
        }
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("Transaction date cannot be in the future");
                }
                _transactionDate = value;
            }
        }
        public Member Member
        {
            get { return _member; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Member cannot be null");
                }
                _member = value;
            }
        }
        public abstract void Execute();
    }
}
