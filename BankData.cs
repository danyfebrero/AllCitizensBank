﻿using System;
using System.Collections.Generic;

namespace AllCitizensBank
{
    public class BankData
    {
        private static int BankAccountSeed { get; set; }
        public List<string> AccountTypes = new() {"Checking", "Savings"}; 

        public int GetNewAccountNumber()
        {
            return BankAccountSeed;
        }
        public void MakeNewSeed()
        {
            BankAccountSeed++;
        }
    }
}
