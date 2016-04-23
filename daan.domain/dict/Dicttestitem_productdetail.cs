using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daan.domain
{
    [Serializable]
    public sealed class Dicttestitem_productdetail : BaseDomain
    {
        #region Private Members
        private double? finalprice;
        private string issendouttest;
        private double? sendoutcustomerid;
        private Dicttestitem dicttestitem;
      
        #endregion


        #region public Members
        public double? Finalprice
        {
            get { return finalprice; }
            set { finalprice = value; }
        }

        public string Issendouttest
        {
            get { return issendouttest; }
            set { issendouttest = value; }
        }

        public double? Sendoutcustomerid
        {
            get { return sendoutcustomerid; }
            set { sendoutcustomerid = value; }
        }
        public Dicttestitem Dicttestitem
        {
            get { return dicttestitem; }
            set { dicttestitem = value; }
        }
        #endregion

    }
}
