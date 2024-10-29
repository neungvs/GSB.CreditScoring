create FUNCTION [dbo].[lookupMonth]
(@Text_Num varchar(15))
RETURNS varchar(15)
AS
BEGIN
-- return substring(@Text_Num,1,1)+','+substring(@Text_Num,2,len(@Text_num)-1)
return case 
when convert(integer,@Text_Num)=1 then 'มกราคม' 
when convert(integer,@Text_Num)=2 then 'กุมภาพันธ์' 
when convert(integer,@Text_Num)=3 then 'มีนาคม' 
when convert(integer,@Text_Num)=4 then 'เมษายน'
when convert(integer,@Text_Num)=5 then 'พฤษภาคม' 
when convert(integer,@Text_Num)=6 then 'มิถุนายน'
when convert(integer,@Text_Num)=7 then 'กรกฎาคม' 
when convert(integer,@Text_Num)=8 then 'สิงหาคม' 
when convert(integer,@Text_Num)=9 then 'กันยายน' 
when convert(integer,@Text_Num)=10 then 'ตุลาคม'
when convert(integer,@Text_Num)=11 then 'พฤศจิกายน' 
when convert(integer,@Text_Num)=12 then 'ธันวาคม'
end

end