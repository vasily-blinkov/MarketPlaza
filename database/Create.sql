-- This scripts creates the Wholesale database.
-- To populate the database with sample data, execute Sample.sql.

USE [master];

-- Wholesale.
/*
USE master; DROP DATABASE Wholesale;
*/
IF DB_ID('Wholesale') IS NULL
BEGIN
	PRINT N'Creating database ''Wholesale''';
	EXEC ('CREATE DATABASE Wholesale COLLATE Cyrillic_General_CI_AS');
END
GO

USE [Wholesale];

-- To prevent unauthorized access to data in the tables, I create a database role allowing only to execute stored procedures.
/*
drop role executor;
*/
IF NOT EXISTS (SELECT 1 FROM sys.database_principals p WHERE p.name = N'db_executor' AND p.[type] = N'R')
BEGIN
	PRINT N'Setting up database role ''db_executor''.';
	CREATE ROLE db_executor;
	GRANT EXECUTE TO db_executor;
END

-- Creating a server login.
/*
drop login executor;
*/
IF NOT EXISTS (SELECT 1 FROM sys.server_principals p WHERE p.name = N'executor' AND p.[type] = N'S')
BEGIN
	PRINT N'Creating server login ''executor''';
	CREATE LOGIN executor WITH PASSWORD = N'Ver$1l0Ff';
END

-- Creating a new user 'executor' for the server login 'executor'.
/*
drop user executor;
*/
IF NOT EXISTS (SELECT 1 FROM sys.database_principals p WHERE p.name = N'executor' AND p.[type] = N'S')
BEGIN
	PRINT N'Creating database user ''executor''';
	CREATE USER executor FOR LOGIN executor;
END

-- Assigning role executor to user executor.
/*
alter role db_executor drop member executor;
*/
IF NOT EXISTS (SELECT 1 FROM sys.database_role_members rm
	LEFT JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id AND r.type_desc = N'DATABASE_ROLE'
	LEFT JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id AND m.type_desc = N'SQL_USER'
	WHERE r.name = N'db_executor' AND m.name = N'executor')
BEGIN
	PRINT N'Adding user ''executor'' to role ''db_executor''';
	ALTER ROLE db_executor ADD MEMBER executor;
END

-- Wholesale.Administration.
IF SCHEMA_ID('Administration') IS NULL
BEGIN
	PRINT N'Creating schema ''Administration''.';
	EXEC (N'CREATE SCHEMA Administration');
END
GO

-- Wholesale.Administration.User.
IF OBJECT_ID('Administration.User') IS NULL
BEGIN
	PRINT N'Creating table ''User''.'
	CREATE TABLE Administration.[User] (
		ID smallint IDENTITY(-32768, 1) PRIMARY KEY NOT NULL,
		FullName nvarchar(150) NOT NULL,
		LoginName nvarchar(150) NOT NULL UNIQUE,
		PasswordHash nvarchar(128) NOT NULL,
		Description nvarchar(1500),
		CreatedBy smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		CreatedDate datetime DEFAULT GETDATE() NOT NULL,
		UpdatedBy smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		UpdatedDate datetime DEFAULT GETDATE() NOT NULL,
		IsDeleted bit DEFAULT 0 NOT NULL
	);
	EXEC sp_addextendedproperty
		@name = N'MS_Description', @value = N'SHA2-512',
		@level0type = N'SCHEMA',   @level0name = N'Administration',
		@level1type = N'TABLE',    @level1name = N'User',
		@level2type = N'COLUMN',   @level2name = N'PasswordHash';
END
GO

-- IX_User_FullName (Wholesale.Administration).
IF NOT EXISTS(SELECT 1 FROM sys.indexes i WHERE i.name = N'IX_User_FullName')
BEGIN
	PRINT 'Creating index ''IX_User_FullName''.';
	CREATE NONCLUSTERED INDEX IX_User_FullName
		ON Administration.[User] (FullName);
END
GO

-- Wholesale.Administration.User: seed.
IF NOT EXISTS(SELECT * FROM Administration.[User] u WHERE u.ID = -32768)
BEGIN
	PRINT 'Creating user ''seed'' (password: seed).';
	DECLARE @password_salt nvarchar(20) = N'woTdzTfu5VUxUjtnr8fJ';
	INSERT INTO Administration.[User](FullName, LoginName, PasswordHash, CreatedBy, UpdatedBy)
	VALUES(
		N'Seed', N'seed',
		CONVERT(nvarchar(128), HashBytes('sha2_512', @password_salt + 'seed'), 2),
		-32768, -32768
	);
END
GO

-- Wholesale.Administration.Role.
IF OBJECT_ID('Administration.Role') IS NULL
BEGIN
	PRINT N'Creating table ''Role''.'
	CREATE TABLE Administration.[Role] (
		ID smallint IDENTITY(-32768, 1) PRIMARY KEY NOT NULL,
		Name nvarchar(150) NOT NULL,
		Description nvarchar(1500),
		CreatedBy smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		CreatedDate datetime DEFAULT GETDATE() NOT NULL,
		UpdatedBy smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		UpdatedDate datetime DEFAULT GETDATE() NOT NULL,
		IsDeleted bit DEFAULT 0 NOT NULL
	);
END
GO

-- Wholesale.Administration.Role: Администратор ИС.
IF NOT EXISTS(SELECT * FROM Administration.[Role] r WHERE r.ID = -32768)
BEGIN
	PRINT 'Creating role ''Администратор ИС''.';
	DECLARE @user_id smallint;
	SELECT @user_id = u.ID FROM Administration.[User] u WHERE u.LoginName = N'seed';
	INSERT INTO Administration.[Role](Name, Description, CreatedBy, UpdatedBy)
		VALUES(N'Администратор ИС', N'Администратор информационной системы', @user_id, @user_id);
END
GO

-- Wholesale.Administration.Role: Администратор безопасности ИС.
IF NOT EXISTS(SELECT * FROM Administration.[Role] u WHERE u.ID = -32767)
BEGIN
	PRINT 'Creating role ''Администратор безопасности ИС''.';
	DECLARE @user_id smallint;
	SELECT @user_id = u.ID FROM Administration.[User] u WHERE u.LoginName = N'seed';
	INSERT INTO Administration.[Role](Name, Description, CreatedBy, UpdatedBy)
		VALUES(N'Администратор безопасности ИС', N'Администратор безопасности информационной системы', @user_id, @user_id);
END
GO

-- Wholesale.Administration.Role: Оператор.
IF NOT EXISTS(SELECT * FROM Administration.[Role] u WHERE u.ID = -32766)
BEGIN
	PRINT 'Creating role ''Оператор''.';
	DECLARE @user_id smallint;
	SELECT @user_id = u.ID FROM Administration.[User] u WHERE u.LoginName = N'seed';
	INSERT INTO Administration.[Role](Name, Description, CreatedBy, UpdatedBy)
		VALUES(N'Оператор', N'Оператор', @user_id, @user_id);
END
GO

-- Wholesale.Administration.Role: Привилегированный пользователь.
IF NOT EXISTS(SELECT * FROM Administration.[Role] u WHERE u.ID = -32765)
BEGIN
	PRINT 'Creating role ''Привилегированный пользователь''.';
	DECLARE @user_id smallint;
	SELECT @user_id = u.ID FROM Administration.[User] u WHERE u.LoginName = N'seed';
	INSERT INTO Administration.[Role](Name, Description, CreatedBy, UpdatedBy)
		VALUES(N'Привилегированный пользователь', N'Привилегированный пользователь', @user_id, @user_id);
END
GO

-- Wholesale.Administration.Role: Непривилегированный пользователь.
IF NOT EXISTS(SELECT * FROM Administration.[Role] u WHERE u.ID = -32764)
BEGIN
	PRINT 'Creating role ''Непривилегированный пользователь''.';
	DECLARE @user_id smallint;
	SELECT @user_id = u.ID FROM Administration.[User] u WHERE u.LoginName = N'seed';
	INSERT INTO Administration.[Role](Name, Description, CreatedBy, UpdatedBy)
		VALUES(N'Непривилегированный пользователь', N'Непривилегированный пользователь', @user_id, @user_id);
END
GO

-- Wholesale.Administration.UserRole.
IF OBJECT_ID('Administration.UserRole') IS NULL
BEGIN
	PRINT N'Creating table ''UserRole''.'
	CREATE TABLE Administration.UserRole (
		UserID smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		RoleID smallint FOREIGN KEY REFERENCES Administration.[Role](ID) NOT NULL,
		CreatedBy smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		CreatedDate datetime DEFAULT GETDATE() NOT NULL,
		UpdatedBy smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		UpdatedDate datetime DEFAULT GETDATE() NOT NULL,
		IsDeleted bit DEFAULT 0 NOT NULL,
		PRIMARY KEY (UserID, RoleID)
	);
END
GO

-- Granting role Администратор to user 'seed'.
IF NOT EXISTS(SELECT * FROM Administration.UserRole ur WHERE ur.UserID = -32768 AND ur.RoleID = -32768)
BEGIN
	PRINT 'Granting role ''Администратор'' to user ''seed''.'
	INSERT INTO Administration.UserRole(UserID, RoleID, CreatedBy, UpdatedBy)
	VALUES(-32768, -32768, -32768, -32768);
END
GO

-- Wholesale.Auth.
IF SCHEMA_ID('Auth') IS NULL
BEGIN
	PRINT N'Creating schema ''Auth''.';
	EXEC (N'CREATE SCHEMA Auth');
END
GO

-- Wholesale.Auth.Session.
IF OBJECT_ID('Auth.Session') IS NULL
BEGIN
	PRINT N'Creating table ''Session''.'
	CREATE TABLE Auth.Session (
		Token nvarchar(128) PRIMARY KEY NOT NULL,
		UserID smallint FOREIGN KEY REFERENCES Administration.[User](ID) NOT NULL,
		UpdatedDate datetime DEFAULT GETDATE() NOT NULL
	);
END
GO

-- Wholesale.MarketPlaza.
IF SCHEMA_ID('MarketPlaza') IS NULL
BEGIN
	PRINT N'Creating schema ''MarketPlaza''.';
	EXEC (N'CREATE SCHEMA MarketPlaza');
END
GO

-- Wholesale.MarketPlaza.Товары.
IF OBJECT_ID('MarketPlaza.Товары') IS NULL
BEGIN
	PRINT N'Creating table ''Товары''.'
	CREATE TABLE MarketPlaza.Товары (
		ТоварID smallint IDENTITY(-32768, 1) PRIMARY KEY NOT NULL,
		Название nvarchar(100) NOT NULL UNIQUE,
		Цена numeric(11, 3) NOT NULL,
		IsDeleted bit DEFAULT 0 NOT NULL
	);
END
GO

-- Wholesale.MarketPlaza.Арендаторы.
IF OBJECT_ID('MarketPlaza.Арендаторы') IS NULL
BEGIN
	PRINT N'Creating table ''Арендаторы''.'
	CREATE TABLE MarketPlaza.Арендаторы (
		АрендаторID smallint IDENTITY(-32768, 1) PRIMARY KEY NOT NULL,
		Фамилия nvarchar(100) NOT NULL,
		Имя nvarchar(100) NOT NULL,
		Отчество nvarchar(100) NOT NULL,
		Телефон nvarchar(22) NOT NULL,
		Адрес nvarchar(1000) NOT NULL,
		IsDeleted bit DEFAULT 0 NOT NULL
	);
END
GO

-- Заполнение арендаторов.
IF NOT EXISTS(SELECT * FROM MarketPlaza.Арендаторы u WHERE u.АрендаторID = -32768)
BEGIN
	PRINT 'Fill table ''Арендаторы''.';
	INSERT INTO MarketPlaza.Арендаторы(Фамилия, Имя, Отчество, Телефон, Адрес) VALUES
		(N'Иванов', N'Иван', N'Иванович', N'+7 (555) 111-11-11', N'Москва, Ленинградское шоссе, дом 3, строение 3'),
		(N'Петров', N'Пётр', N'Петрович', N'+7 (555) 222-22-22', N'Рязань, Вокзальная улица, дом 6, помещение 6'),
		(N'Олегов', N'Олег', N'Олегович', N'+7 (555) 333-33-33', N'Барнаул, Ленина улица, дом 9, квартира 9');
END
GO

-- Procedure: Wholesale.MarketPlaza.GetLessees
PRINT N'Creating or altering procedure ''GetLessees''';
CREATE OR ALTER PROCEDURE MarketPlaza.GetLessees
	@query nvarchar(150) = NULL,
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	SELECT u.АрендаторID, u.Фамилия + N' ' + u.Имя + N' ' + u.Отчество FullName 
		FROM MarketPlaza.Арендаторы u
		WHERE @query IS NULL
		OR LOWER(u.Фамилия) LIKE N'%' + LOWER(@query) + '%'
		OR LOWER(u.Имя) LIKE N'%' + LOWER(@query) + '%'
		OR LOWER(u.Отчество) LIKE N'%' + LOWER(@query) + '%'
		OR LOWER(u.Телефон) LIKE N'%' + LOWER(@query) + '%'
		OR LOWER(u.Адрес) LIKE N'%' + LOWER(@query) + '%';
END
GO

-- Procedure: Wholesale.MarketPlaza.GetSignleLessee
PRINT N'Creating or altering procedure ''GetSignleLessee''';
CREATE OR ALTER PROCEDURE MarketPlaza.GetSignleLessee
	@id smallint,
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	SELECT TOP(1)
		u.АрендаторID, u.Фамилия, u.Имя, u.Отчество, u.Телефон, u.Адрес 
		FROM MarketPlaza.Арендаторы u
		WHERE u.АрендаторID = @id AND u.IsDeleted = 0;
END
GO

-- Procedure: Wholesale.MarketPlaza.AddLessee
PRINT N'Creating or altering procedure ''AddLessee''';
CREATE OR ALTER PROCEDURE MarketPlaza.AddLessee
	@json nvarchar(max),
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	-- Parse input JSON.
	DECLARE @new_entity TABLE(Фамилия nvarchar(100), Имя nvarchar(100), Отчество nvarchar(100), Телефон nvarchar(22), Адрес nvarchar(1000));
	INSERT @new_entity (Фамилия, Имя, Отчество, Телефон, Адрес)
		SELECT j.Фамилия, j.Имя, j.Отчество, j.Телефон, j.Адрес
		FROM OpenJson(@json)
		WITH (Фамилия nvarchar(100), Имя nvarchar(100), Отчество nvarchar(100), Телефон nvarchar(22), Адрес nvarchar(1000)) AS j;
	-- Validating inserting entity.
	IF EXISTS (SELECT 1 FROM @new_entity u WHERE
		ISNULL(u.Фамилия, N'') = N'' OR ISNULL(u.Имя, N'') = N'' OR ISNULL(u.Отчество, N'') = N'' OR ISNULL(u.Телефон, N'') = N'' OR ISNULL(u.Адрес, N'') = N'')
		THROW 54000, N'Не указано значение одного из обязательных для создания нового арендателя полей, таких как: фамилия, имя, отчество, телефон, адрес', 1;
	-- Current user ID.
	DECLARE @user_id smallint;
	SELECT @user_id = s.UserID FROM Auth.[Session] s WHERE s.Token = @token;
	-- Inserting an entity.
	DECLARE @inserted_entity TABLE(ID smallint);
	BEGIN TRY
		BEGIN TRANSACTION
			INSERT MarketPlaza.Арендаторы (Фамилия, Имя, Отчество, Телефон, Адрес)
				OUTPUT INSERTED.АрендаторID INTO @inserted_entity
				SELECT j.Фамилия, j.Имя, j.Отчество, j.Телефон, j.Адрес
				FROM @new_entity j;
		COMMIT;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK;
			THROW 54001, N'Произошло исключение при сохранении информации об арендаторе', 1;
		END
	END CATCH
END
GO

-- Procedure: Wholesale.MarketPlaza.EditLessee
PRINT N'Creating or altering procedure ''EditLessee''';
CREATE OR ALTER PROCEDURE MarketPlaza.EditLessee
	@json nvarchar(max),
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	-- Parsing input JSON.
	DECLARE @changes TABLE(АрендаторID smallint, Фамилия nvarchar(100), Имя nvarchar(100), Отчество nvarchar(100), Телефон nvarchar(22), Адрес nvarchar(1000));
	INSERT @changes(АрендаторID, Фамилия, Имя, Отчество, Телефон, Адрес)
		SELECT j.АрендаторID, j.Фамилия, j.Имя, j.Отчество, j.Телефон, j.Адрес FROM OpenJson(@json)
		WITH (АрендаторID smallint, Фамилия nvarchar(100), Имя nvarchar(100), Отчество nvarchar(100), Телефон nvarchar(22), Адрес nvarchar(1000)) AS j;
	-- ID of the entity to edit.
	DECLARE @id smallint;
	SELECT @id = u.АрендаторID FROM @changes u;
	-- Validate input data.
	IF NOT EXISTS (SELECT 1 FROM MarketPlaza.Арендаторы u WHERE u.АрендаторID = @id)
	BEGIN
		DECLARE @error nvarchar(100) = N'Редактирование не выполнено, так как арендатор с ID = "'
			+ IIF(@id IS NULL, N'(не указан)', CONVERT(nvarchar(6), @id))
			+ N'" не существует';
		THROW 55000, @error, 1;
	END
	BEGIN TRY
		BEGIN TRANSACTION
			-- Updating the entity.
			UPDATE MarketPlaza.Арендаторы SET
				Фамилия = IIF(TRIM(ISNULL(json.Фамилия, N'')) = N'', MarketPlaza.Арендаторы.Фамилия , json.Фамилия),
				Имя = IIF(TRIM(ISNULL(json.Имя, N'')) = N'', MarketPlaza.Арендаторы.Имя , json.Имя),
				Отчество = IIF(TRIM(ISNULL(json.Отчество, N'')) = N'', MarketPlaza.Арендаторы.Отчество , json.Отчество),
				Адрес = IIF(TRIM(ISNULL(json.Адрес, N'')) = N'', MarketPlaza.Арендаторы.Адрес , json.Адрес),
				Телефон = IIF(TRIM(ISNULL(json.Телефон, N'')) = N'', MarketPlaza.Арендаторы.Телефон , json.Телефон)
			FROM @changes AS json WHERE MarketPlaza.Арендаторы.АрендаторID = @id;
		COMMIT
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK;
			THROW 55001, N'Редактирование арендатора не выполнено', 1;
		END
	END CATCH
END
GO

-- Wholesale.MarketPlaza.Места.
IF OBJECT_ID('MarketPlaza.Места') IS NULL
BEGIN
	PRINT N'Creating table ''Места''.'
	CREATE TABLE MarketPlaza.Места (
		МестоID smallint IDENTITY(-32768, 1) PRIMARY KEY NOT NULL,
		Размер numeric(6, 1) NOT NULL,
		Класс nvarchar(20) NOT NULL,
		Цена numeric(11, 3) NOT NULL,
		Адрес nvarchar(1000) NOT NULL,
		Номер nvarchar(20) NOT NULL,
		IsDeleted bit DEFAULT 0 NOT NULL
	);
END
GO

-- Wholesale.MarketPlaza.ЗанятиеМест.
IF OBJECT_ID('MarketPlaza.ЗанятиеМест') IS NULL
BEGIN
	PRINT N'Creating table ''ЗанятиеМест''.'
	CREATE TABLE MarketPlaza.ЗанятиеМест (
		ДатаНачала datetime2 NOT NULL,
		ДатаОкончания datetime2 NOT NULL,
		МестоID smallint FOREIGN KEY REFERENCES MarketPlaza.Места(МестоID) NOT NULL,
		АрендаторID smallint FOREIGN KEY REFERENCES MarketPlaza.Арендаторы(АрендаторID) NOT NULL,
		PRIMARY KEY (МестоID, АрендаторID)
	);
END
GO

-- Wholesale.MarketPlaza.ТоварАрендатор.
IF OBJECT_ID('MarketPlaza.ТоварАрендатор') IS NULL
BEGIN
	PRINT N'Creating table ''ТоварАрендатор''.'
	CREATE TABLE MarketPlaza.ТоварАрендатор (
		ТоварID smallint FOREIGN KEY REFERENCES MarketPlaza.Арендаторы(АрендаторID) NOT NULL,
		АрендаторID smallint FOREIGN KEY REFERENCES MarketPlaza.Товары(ТоварID) NOT NULL,
		PRIMARY KEY (ТоварID, АрендаторID)
	);
END
GO

-- Function: Wholesale.Auth.DoesLoginExist.
PRINT N'Creating or altering function ''DoesLoginExist''';
CREATE OR ALTER FUNCTION Auth.DoesLoginExist(@login_name nvarchar(150)) RETURNS bit
BEGIN
	DECLARE @exists bit;
	SELECT @exists = IIF(COUNT(1) = 1, 1, 0) FROM Administration.[User] u
		WHERE u.LoginName = @login_name;
	RETURN @exists;
END
GO

-- Procedure: Wholesale.Auth.Authenticate.
PRINT N'Creating or altering procedure ''Authenticate''';
CREATE OR ALTER PROCEDURE Auth.Authenticate 
	@login_name nvarchar(150),
	@password_hash nvarchar(128),
	@token nvarchar(128) OUTPUT,
	@user_id smallint OUTPUT
AS BEGIN
	SET NOCOUNT ON; -- for output parameters to be returned to outside
	DECLARE @expected_password_hash nvarchar(128);
	SELECT @expected_password_hash = u.PasswordHash, @user_id = u.ID
		FROM Administration.[User] u WHERE u.LoginName = @login_name;
	IF @expected_password_hash IS NOT NULL AND @expected_password_hash = @password_hash
	BEGIN
		DECLARE @token_salt nvarchar(20) = N'CjWvXV7ZXtHDPyH8y4LV';
		DECLARE @date datetime = GETDATE();
		SET @token = CONVERT(nvarchar(128), HashBytes('SHA2_512',
			@token_salt
			+ CONVERT(nvarchar(6), @user_id)
			+ CONVERT(nvarchar(26), @date, 9)), 2);
		INSERT INTO Auth.[Session] (Token, UserID, UpdatedDate)
			VALUES (@token, @user_id, @date);
	END
END
GO

-- Procedure: Wholesale.Auth.LogOut.
PRINT N'Creating or altering procedure ''LogOut''';
CREATE OR ALTER PROCEDURE Auth.LogOut
	@token nvarchar(128)
AS BEGIN
	DELETE FROM Auth.[Session] WHERE Token = @token;
END
GO

-- Procedure: Wholesale.Auth.CleanupSessions.
-- Removes all sessions older than 2 hours.
PRINT N'Creating or altering procedure ''CleanupSessions''';
CREATE OR ALTER PROCEDURE Auth.CleanupSessions
AS BEGIN
	DELETE FROM Auth.[Session] WHERE DATEADD(hour, 2, UpdatedDate) < GETDATE();
END
GO

-- Procedure: Wholesale.Auth.ValidateToken.
PRINT N'Creating or altering procedure ''ValidateToken''';
CREATE OR ALTER PROCEDURE Auth.ValidateToken
	@token nvarchar(128)
AS BEGIN
	DECLARE @count smallint;
	SELECT @count = COUNT(1) FROM Auth.[Session] s
		WHERE s.Token = @token AND DATEADD(hour, 2, s.UpdatedDate) >= GETDATE();
	IF @count = 1
		UPDATE Auth.[Session] SET UpdatedDate = GETDATE() WHERE Token = @token;
	ELSE
	BEGIN
		DELETE FROM Auth.[Session] WHERE Token = @token;
		THROW 51000, N'Ваша сессия истекла, пройдите повторную аутентификацию', 1;
	END
END
GO

-- Procedure: Wholesale.Administration.GetRoles.
PRINT N'Creating or altering procedure ''GetRoles''';
CREATE OR ALTER PROCEDURE Administration.GetRoles
	@user_id smallint = NULL,
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	SET NOCOUNT ON;
	IF @user_id IS NOT NULL
		-- Select roles for a specific user.
		SELECT ur.RoleID ID, r.Name Name, r.Description Description
		FROM Administration.UserRole ur
		JOIN Administration.Role r ON r.ID = ur.RoleID
		WHERE ur.UserID = @user_id AND r.IsDeleted = 0 AND ur.IsDeleted = 0;
	ELSE
		-- Select all roles available.
	SELECT r.ID ID, r.Name Name, r.Description Description
		FROM Administration.[Role] r
		WHERE r.IsDeleted = 0;
END
GO

-- Procedure: Wholesale.Administration.GetUsers
PRINT N'Creating or altering procedure ''GetUsers''';
CREATE OR ALTER PROCEDURE Administration.GetUsers
	@query nvarchar(150) = NULL,
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	SELECT u.ID, u.FullName, u.LoginName FROM Administration.[User] u
		WHERE @query IS NULL OR u.LoginName LIKE N'%' + @query + '%' OR u.FullName LIKE N'%' + @query + '%';
END
GO

-- Procedure: Wholesale.Administration.GetSignleUser
PRINT N'Creating or altering procedure ''GetSignleUser''';
CREATE OR ALTER PROCEDURE Administration.GetSignleUser
	@user_id smallint,
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	SELECT TOP(1)
		u.ID, u.FullName, u.LoginName, u.Description
		FROM Administration.[User] u
		WHERE u.ID = @user_id;
END
GO

-- Procedure: Wholesale.Administration.AddUser
PRINT N'Creating or altering procedure ''AddUser''';
CREATE OR ALTER PROCEDURE Administration.AddUser
	@user_json nvarchar(max),
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	-- Parse input JSON.
	DECLARE @new_user TABLE(FullName nvarchar(150), LoginName nvarchar(150), PasswordHash nvarchar(128), Description nvarchar(1500));
	INSERT @new_user (FullName, LoginName, PasswordHash, Description)
		SELECT j.FullName, j.LoginName, j.PasswordHash, j.Description
		FROM OpenJson(@user_json)
		WITH (FullName nvarchar(150), LoginName nvarchar(150), PasswordHash nvarchar(128), Description nvarchar(1500)) AS j;
	-- Validating inserting user.
	IF EXISTS (SELECT 1 FROM @new_user u WHERE ISNULL(u.LoginName, N'') = N'' OR ISNULL(u.FullName, N'') = N'' OR ISNULL(u.PasswordHash, N'') = N'')
		THROW 52000, N'Не указано значение одного из обязательных для создания нового пользователя полей, таких как: логин, ФИО, пароль', 1;
	-- Current user ID.
	DECLARE @user_id smallint;
	SELECT @user_id = s.UserID FROM Auth.[Session] s WHERE s.Token = @token;
	-- Inserting a user.
	DECLARE @inserted_user TABLE(ID smallint);
	BEGIN TRY
		BEGIN TRANSACTION
			INSERT Administration.[User] (FullName, LoginName, PasswordHash, Description, CreatedBy, UpdatedBy)
				OUTPUT INSERTED.ID INTO @inserted_user
				SELECT j.FullName, j.LoginName, j.PasswordHash, j.Description, @user_id CreatedBy, @user_id UpdatedBy
				FROM @new_user j;
			-- Inserted user ID.
			DECLARE @new_id smallint;
			SELECT @new_id = u.ID FROM @inserted_user u;
			-- Inserting roles.
			INSERT Administration.UserRole (UserID, RoleID, CreatedBy, UpdatedBy)
				SELECT @new_id, j.value, @user_id, @user_id
				FROM OpenJson(@user_json, N'$.Roles') j;
		COMMIT;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK;
			THROW;
		END
	END CATCH
END
GO

-- (INTERNAL USAGE ONLY) Procedue: Wholesale.Administration.EditUserRoles
PRINT N'Creating or altering procedure ''EditUserRoles''';
CREATE OR ALTER PROCEDURE Administration.EditUserRoles
	@roles_json nvarchar(max), -- E.g.: [-32768, -32767, -32766]
	@editing_user_id smallint, -- ID of the user we trying to edit
	@current_user_id smallint, -- Previously obtained by the @token ID of the current user (to eliminate excessive request to the sessions table)
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	IF @roles_json IS NOT NULL
	BEGIN
		-- Parse roles.
		DECLARE @roles TABLE(ID smallint);
		INSERT @roles (ID) SELECT r.value FROM OpenJson(@roles_json) r;
		-- Revoke roles.
		UPDATE Administration.UserRole SET IsDeleted = 1, UpdatedBy = @current_user_id, UpdatedDate = GETDATE()
			WHERE Administration.UserRole.UserID = @editing_user_id
			AND Administration.UserRole.IsDeleted = 0
			AND NOT EXISTS (SELECT 1 FROM @roles r WHERE r.ID = Administration.UserRole.RoleID);
		-- Insert roles.
		INSERT Administration.UserRole (UserID, RoleID, CreatedBy, UpdatedBy)
			SELECT @editing_user_id, r.ID, @current_user_id, @current_user_id FROM @roles r
			WHERE NOT EXISTS (SELECT 1 FROM Administration.UserRole ur WHERE ur.UserID = @editing_user_id AND ur.RoleID = r.ID);
		-- Undelete roles.
		UPDATE Administration.UserRole SET IsDeleted = 0, CreatedBy = @current_user_id, CreatedDate = GETDATE(), UpdatedBy = @current_user_id, UpdatedDate = GETDATE()
			WHERE Administration.UserRole.UserID = @editing_user_id AND Administration.UserRole.IsDeleted = 1
			AND EXISTS (SELECT 1 FROM @roles r WHERE r.ID = Administration.UserRole.RoleID);
	END
END
GO

-- Procedure: Wholesale.Administration.EditUser
PRINT N'Creating or altering procedure ''EditUser''';
CREATE OR ALTER PROCEDURE Administration.EditUser
	@user_json nvarchar(max),
	@token nvarchar(128)
AS BEGIN
	EXEC Auth.ValidateToken @token = @token;
	-- Parsing input JSON.
	DECLARE @changes TABLE(ID smallint, FullName nvarchar(150), LoginName nvarchar(150), Description nvarchar(1500), PasswordHash nvarchar(128), Roles nvarchar(max));
	INSERT @changes(ID, FullName, LoginName, Description, PasswordHash, Roles)
		SELECT j.ID, j.FullName, j.LoginName, j.Description, j.PasswordHash, j.Roles FROM OpenJson(@user_json)
		WITH (ID smallint, FullName nvarchar(150), LoginName nvarchar(150), Description nvarchar(1500), PasswordHash nvarchar(128), Roles nvarchar(max) AS JSON) AS j;
	-- ID of the user to edit.
	DECLARE @id smallint;
	SELECT @id = u.ID FROM @changes u;
	-- Validate input data.
	IF NOT EXISTS (SELECT 1 FROM Administration.[User] u WHERE u.ID = @id)
	BEGIN
		DECLARE @error nvarchar(100) = N'Редактирование не выполнено, так как пользователь с ID = "'
			+ IIF(@id IS NULL, N'(не указан)', CONVERT(nvarchar(6), @id))
			+ N'" не существует';
		THROW 53000, @error, 1;
	END	
	-- Current user ID.
	DECLARE @user_id smallint;
	SELECT @user_id = s.UserID FROM Auth.[Session] s WHERE s.Token = @token;
	BEGIN TRY
		BEGIN TRANSACTION
			-- Updating the user.
			UPDATE Administration.[User] SET
				FullName = IIF(TRIM(ISNULL(json.FullName, N'')) = N'', Administration.[User].FullName, json.FullName),
				LoginName = IIF(TRIM(ISNULL(json.LoginName, N'')) = N'', Administration.[User].LoginName, json.LoginName),
				Description = ISNULL(json.Description, Administration.[User].Description),
				PasswordHash = IIF(TRIM(ISNULL(json.PasswordHash, N'')) = N'', Administration.[User].PasswordHash, json.PasswordHash),
				UpdatedBy = @user_id,
				UpdatedDate = GetDate()
			FROM @changes AS json WHERE Administration.[User].ID = @id;
			-- Editing roles.
			DECLARE @roles_json nvarchar(max);
			SELECT @roles_json = c.Roles FROM @changes c;
			EXEC Administration.EditUserRoles @roles_json = @roles_json, @editing_user_id = @id, @current_user_id = @user_id, @token = @token;
		COMMIT
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK;
			THROW;
		END
	END CATCH
END
GO































