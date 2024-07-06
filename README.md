Delete FROM [EnsekCodingExercise].[dbo].[__EFMigrationsHistory];
Drop table [EnsekCodingExercise].[dbo].Readings;
Drop table [EnsekCodingExercise].[dbo].Accounts;

# Ensek-Coding-Exercise
Coding exercise for ENSEK

## Task description
Ensuring the Acceptance Criteria are met, build a C# Web API that connects to an instance of a database and persists the contents of the Meter Reading CSV file. 

We have provided you with: 
* A list of test customers along with their respective Account IDs 
* Please refer to Test_Accounts.csv 
* Please seed the Test_Accounts.csv data into your chosen data storage technology and validate the meter read data against the accounts

## User Story
As an Energy Company Account Manager, I want to be able to load a CSV file of Customer Meter Readings so that we can monitor their energy consumption and charge them accordingly.

## Acceptance Criteria

### Must haves
- [x] Create the following endpoint - POST => /meter-reading-uploads 
- [x] The endpoint should be able to process a CSV of meter readings. An example CSV file has been provided (Meter_reading.csv) 
- [x] Each entry in the CSV should be validated and if valid, stored in a DB. 
- [x] After processing, the number of successful/failed readings should be returned. 

### Validation
- [x] You should not be able to load the same entry twice 
- [x] A meter reading must be associated with an Account ID to be deemed valid 
- [x] Reading values should be in the format NNNNN 

### Nice to have
- [x] Create a client in the technology of your choosing to consume the API. You can use angular/react/whatever you like 
- [x] When an account has an existing read, ensure the new read isn’t older than the existing read

## Tips
We want you to be able to give of your best in this exercise so here are some pointers on what we look for when we mark it: 

* Readable, self-explanatory code 
* Adherence to SOLID principles 
* The creation of clearly testable code 
* Evidence of thorough unit testing 
* Easily maintainable code 
* Having a user interface is a bonus 

Overall, we are looking for clarity of code, understanding of key and core modern coding principles and for you to put your mark on the exercise and enjoy it along the way
