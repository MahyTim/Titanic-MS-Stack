CREATE VIEW STEP_2_MoreGeneralized
AS
SELECT [PassengerId]
      ,[Survived]
      ,[Class]
      ,[LastName]
      ,[FirstName]
      ,(CASE
		WHEN LTRIM( rtrim([Title])) IN ('Don','Major','Capt','Jonkheer','Rev','Col','Master','Sir')
			THEN 'Mr'
		WHEN  LTRIM( rtrim([Title])) IN ('the Countess','countess','Mme','Lady')
		    THEN 'Mrs'
		WHEN  LTRIM( rtrim([Title])) IN ('Mlle','Ms')
			THEN 'Miss'
		WHEN  LTRIM( rtrim([Title])) = 'Dr' AND [sex] = 'male'
			THEN 'Mr'
		WHEN  LTRIM( rtrim([Title])) = 'Dr' AND [sex] = 'female'
		    THEN 'Mrs'
		ELSE  LTRIM( rtrim([Title]))
			END) as 'Title'
      ,[Sex]
      ,[Age]
      ,([NumberOfParentsChildren] + [NumberOfSiblings]) as 'FamilySize'
      ,[Ticket]
      ,([Fare] / ([NumberOfParentsChildren] + [NumberOfSiblings] + 1)) as 'FarePerPerson'
      , (Case 
			WHEN [Cabin] = '' 
					THEN 'U' 
			ELSE SUBSTRING([Cabin],1,1) 
			END) as [Deck]
      ,[Embarked]
  FROM [dbo].[STEP_1_TypedAndCleaned]
