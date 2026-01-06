using System;
using LendingApi.Core.Entities;

namespace LendingApi.Application.Helpers;

public class LoanHelper
{
    public bool LoanBusinessalidation(Loan loan)
    {   
        if (loan.PrincipalAmount < 10000 || loan.InterestRate < 1 || loan.TermsMonth > 60)
            return false;

        return true;
    }
}
