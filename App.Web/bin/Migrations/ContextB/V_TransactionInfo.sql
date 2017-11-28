IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[V_TransactionInfo]'))	
BEGIN
	DROP VIEW dbo.V_TransactionInfo
END
GO
CREATE VIEW V_TransactionInfo 
AS
SELECT  
	ti.Id, ti.TransactionId, 
	CASE
		WHEN ti.TransactionType=1 THEN 'TRANSFER'
		WHEN ti.TransactionType=2 THEN 'DEPOSIT'
		WHEN ti.TransactionType=3 THEN 'EXPENSE'
	END TransactionType,
	(SELECT AccountName FROM BankAccounts WHERE Id=ti.AccountFrom) AccountFrom,
	(SELECT AccountName FROM BankAccounts WHERE Id=ti.AccountTo) AccountTo,
	ti.Date,
	(SELECT MethodName FROM PaymentMethods WHERE Id=ti.MethodId) Payer,
	CASE WHEN ti.TransactionType=1 THEN ti.Amount ELSE 0.00 END TransferAmount,
	CASE WHEN ti.TransactionType=2 THEN ti.Amount ELSE 0.00 END DepositAmount,
	CASE WHEN ti.TransactionType=3 THEN ti.Amount ELSE 0.00 END ExpenseAmount,
	ti.Description
FROM TransactionsInfoes ti
	
