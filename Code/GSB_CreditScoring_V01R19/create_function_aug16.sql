create FUNCTION [dbo].[lookupMonth]
(@Text_Num varchar(15))
RETURNS varchar(15)
AS
BEGIN
-- return substring(@Text_Num,1,1)+','+substring(@Text_Num,2,len(@Text_num)-1)
return case 
when convert(integer,@Text_Num)=1 then '���Ҥ�' 
when convert(integer,@Text_Num)=2 then '����Ҿѹ��' 
when convert(integer,@Text_Num)=3 then '�չҤ�' 
when convert(integer,@Text_Num)=4 then '����¹'
when convert(integer,@Text_Num)=5 then '����Ҥ�' 
when convert(integer,@Text_Num)=6 then '�Զع�¹'
when convert(integer,@Text_Num)=7 then '�á�Ҥ�' 
when convert(integer,@Text_Num)=8 then '�ԧ�Ҥ�' 
when convert(integer,@Text_Num)=9 then '�ѹ��¹' 
when convert(integer,@Text_Num)=10 then '���Ҥ�'
when convert(integer,@Text_Num)=11 then '��Ȩԡ�¹' 
when convert(integer,@Text_Num)=12 then '�ѹ�Ҥ�'
end

end