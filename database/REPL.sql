use master
use Wholesale


-- Administration.

select ur.UserID, ur.RoleID from Administration.UserRole ur

select r.Name from Administration.[Role] r 


-- SQL.

select lower(N'ФамИлиЯ')


-- Арендаторы.

select а.Фамилия FROM MarketPlaza.Арендаторы а WHERE а.АрендаторID = -32747


-- Товары

select g.Название from MarketPlaza.Товары g

EXEC MarketPlaza.EditGood 
