CREATE TABLE [dbo].[AppUser]
(
	[AutoId] BIGINT NOT NULL  IDENTITY(1000,1), 
    [UserId] VARCHAR(30) NOT NULL, 
    [AccountName] NVARCHAR(50) NOT NULL, 
    [PhoneNo] NVARCHAR(20) NOT NULL, 
    [PassKey] NVARCHAR(50) NOT NULL, 
    [RealName] NVARCHAR(20) NULL, 
    [NickName] NVARCHAR(50) NULL, 
    [IDCardNo] NVARCHAR(20) NULL, 
    [ContactAddress] NVARCHAR(50) NULL, 
    [WangWangNo] NVARCHAR(20) NULL, 
    [WeiXinNo] NVARCHAR(50) NULL, 
    [AlipayNo] NVARCHAR(50) NULL, 
    [QQNo] NVARCHAR(20) NULL, 
    [AltPhoneNo] NVARCHAR(20) NULL, 
    CONSTRAINT [PK_AppUser] PRIMARY KEY ([AutoId], [UserId])
)
