USE [training]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[app_user_item_folder]') AND type in (N'U'))
ALTER TABLE [dbo].[app_user_item_folder] DROP CONSTRAINT IF EXISTS [FK_app_user_item_folder_cooperator]
GO
/****** Object:  Table [dbo].[app_user_item_folder]    Script Date: 1/4/2022 12:09:29 PM ******/
DROP TABLE IF EXISTS [dbo].[app_user_item_folder]
GO
/****** Object:  Table [dbo].[app_user_item_folder]    Script Date: 1/4/2022 12:09:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[app_user_item_folder](
	[app_user_item_folder_id] [int] IDENTITY(1,1) NOT NULL,
	[folder_name] [nvarchar](250) NOT NULL,
	[folder_type] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[note] [nvarchar](2000) NULL,
	[is_shared] [bit] NULL,
	[is_favorite] [bit] NULL,
	[created_by] [int] NULL,
	[created_date] [datetime] NULL,
	[modified_by] [int] NULL,
	[modified_date] [datetime] NULL,
 CONSTRAINT [PK_app_user_item_folder] PRIMARY KEY CLUSTERED 
(
	[app_user_item_folder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[app_user_item_folder]  WITH CHECK ADD  CONSTRAINT [FK_app_user_item_folder_cooperator] FOREIGN KEY([created_by])
REFERENCES [dbo].[cooperator] ([cooperator_id])
GO
ALTER TABLE [dbo].[app_user_item_folder] CHECK CONSTRAINT [FK_app_user_item_folder_cooperator]
GO
