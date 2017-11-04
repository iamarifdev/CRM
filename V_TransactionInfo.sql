USE [CRM_DB]
GO

/****** Object:  View [dbo].[V_TransactionInfo]    Script Date: 11/4/2017 2:12:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER VIEW [dbo].[V_TransactionInfo] 
AS
	SELECT  
		ti.Id, ti.TransactionId, 
		CASE
			WHEN ti.TransactionType=1 THEN 'TRANSFER'
			WHEN ti.TransactionType=2 THEN 'DEPOSIT'
			WHEN ti.TransactionType=3 THEN 'EXPENSE'
		END TransactionType,
		ISNULL((SELECT AccountName FROM BankAccounts WHERE Id=ti.AccountFrom),'') AccountFrom,
		ISNULL((SELECT AccountName FROM BankAccounts WHERE Id=ti.AccountTo),'') AccountTo,
		ISNULL(ti.Date, '') Date,
		CASE
			WHEN ti.PayerType=1 THEN (SELECT AgentName FROM AgentInfoes WHERE Id=ti.PayerId)
			WHEN ti.PayerType=2 THEN (SELECT FirstName FROM ClientInfoes WHERE Id=ti.PayerId)
			WHEN ti.PayerType=3 THEN 'Officer'
			WHEN ti.PayerType=4 THEN 'Other'
			ELSE ''
		END Payer,
		(SELECT MethodName FROM PaymentMethods WHERE Id=ti.MethodId) Method,
		CASE WHEN ti.TransactionType=1 THEN ti.Amount ELSE 0.00 END TransferAmount,
		CASE WHEN ti.TransactionType=2 THEN ti.Amount ELSE 0.00 END DepositAmount,
		CASE WHEN ti.TransactionType=3 THEN ti.Amount ELSE 0.00 END ExpenseAmount,
		ti.Description
	FROM TransactionsInfoes ti

GO


