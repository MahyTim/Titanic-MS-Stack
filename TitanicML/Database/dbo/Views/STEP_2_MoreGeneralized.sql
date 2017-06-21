CREATE VIEW [dbo].[STEP_2_MoreGeneralized]
WITH SCHEMABINDING
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
      ,([NumberOfParentsChildren] + [NumberOfSiblings] + 1) as 'FamilySize'
      ,[Ticket]
      ,([Fare]) as 'FarePerPerson'
      , (Case 
			WHEN [Cabin] = '' 
					THEN 'U' 
			WHEN SUBSTRING([Cabin],1,1) = 'T'
				THEN 'U'
			ELSE SUBSTRING([Cabin],1,1) 
			END) as [Deck]
      ,(
	  CASE 
		WHEN [Embarked]  = 'S'
			THEN 'Southampton'
		WHEN [Embarked] = 'Q'
			THEN 'Queenstown'
		ELSE
			'Cherbourg'
		END) as [Embarked],
		(
			CASE
			WHEN (  SELECT COUNT(0) from dbo.STEP_1_TypedAndCleaned s3
						WHERE 
						(
							s1.Sex = 'female' AND s1.Age > 18
						)
						AND 
						(
							(  
							 	(((s1.[NumberOfParentsChildren] + s1.[NumberOfSiblings] + 1) = 2) 
									AND NOT EXISTS ( select top 1 1 from dbo.STEP_1_TypedAndCleaned s2 where s2.Ticket = s1.Ticket and s2.Sex = 'male' ))
							)
							OR
							( 
								((s1.[NumberOfParentsChildren] + s1.[NumberOfSiblings] + 1) > 2 
									AND EXISTS (select top 1 1 from dbo.STEP_1_TypedAndCleaned s2 where s2.Ticket = s1.Ticket AND (s2.Age < 18) AND (s2.Age < s1.Age) )
							)) 
						)
					) > 1	
				THEN 1
			ELSE
				0
			END ) AS [IsMother]
  FROM [dbo].[STEP_1_TypedAndCleaned] s1
  WHERE [Fare] <> 0 AND [Embarked] IN ('S','C','Q') And Age <> 0
GO