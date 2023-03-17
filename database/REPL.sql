use master
use Wholesale

-- Administration.
select ur.UserID, ur.RoleID from Administration.UserRole ur
select r.Name from Administration.[Role] r
select u.passwordhash from Administration.[User] u where u.LoginName = N'seed'
exec Administration.GetRoles
	@user_id = -32768,
	@token = N'FA83A9ECB287B495E65393863499CE171C897A92F44B1A46A71988A5ADF789D87A059DB09F460D8B8E1BFD4DFCB5AABDCF4EA7098D3094012834FEBF5A40E1A5'

-- SQL.
select lower(N'ФамИлиЯ')

-- Арендаторы.
select а.АрендаторID, а.Фамилия FROM MarketPlaza.Арендаторы а WHERE а.АрендаторID = -32747

-- Товары
select g.ТоварID, g.Название from MarketPlaza.Товары g

-- Аутентификация.
declare @token nvarchar(128);
declare @user_id smallint;
EXEC Auth.Authenticate
	@login_name = N'seed',
	@password_hash = N'B7810BEB441A313E7771C9B4E5560469E8B3AFA55FC537BD8CC563B8EE06C9A4E88E9B0A42F94749C81A82FED266949F6AD45BBBBED09760B7D50316AD3293F8',
	@token = @token OUTPUT,
	@user_id = @user_id OUTPUT;
PRINT N'Token: ' + @token;

-- ТоварАрендатор
declare @goods table(ТоварID smallint, Название nvarchar(100), Цена numeric(11, 3));
select * from MarketPlaza.ТоварАрендатор
delete from MarketPlaza.ТоварАрендатор
exec MarketPlaza.GetLesseeGoods
	@lessee_id = -32766,
	@token = N'83126E932994AFFB28FCE2626885F933906A804759CC41446E2FE371C913F5B4D7D6D28B436596081162DF7387A65D53F1CB63FF49CE6B216546887E89133AA7';
select g.ТоварID, g.Название, g.Цена from @goods g;
