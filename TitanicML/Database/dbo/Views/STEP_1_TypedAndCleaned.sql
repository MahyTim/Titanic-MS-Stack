CREATE VIEW STEP_1_TypedAndCleaned
AS
SELECT TOP (1000) [PassengerId]
      ,cast([Survived] as bit) as 'Survived'
       ,(CASE 
                  WHEN PClass = '1'
                     THEN 'First'
				  WHEN PClass = '2'
					THEN 'Second'
                  ELSE 'Third'
				  END) as 'Class'
      ,SUBSTRING([Name], 0,CHARINDEX(',',[Name])) as 'LastName'
	  ,SUBSTRING( SUBSTRING([Name], CHARINDEX(',',[Name]) + 1,LEN([Name])),CHARINDEX('.',SUBSTRING([Name], CHARINDEX(',',[Name]) + 1,LEN([Name]))) + 1, LEN(SUBSTRING([Name], CHARINDEX(',',[Name]) + 1,LEN([Name])))) as 'FirstName'
	  ,SUBSTRING(SUBSTRING([Name], CHARINDEX(',',[Name]) + 1,LEN([Name])),0,CHARINDEX('.',SUBSTRING([Name], CHARINDEX(',',[Name]) + 1,LEN([Name])))) as 'Title'
      ,[Sex]
      , (CASE
		WHEN CHARINDEX('.',[Age]) > 0
			THEN
			 CAST(SUBSTRING([Age], 0,CHARINDEX('.',[Age])) as int)
			ELSE
			[Age]
			END
			) as 'Age'
      ,cast([SibSp] as int) as 'NumberOfSiblings'
      ,cast([Parch] as int) as 'NumberOfParentsChildren'
      ,[Ticket]
      ,cast([Fare] as decimal(18,2)) as 'Fare'
      ,[Cabin]
      ,[Embarked]
  FROM [dbo].[RawData]
