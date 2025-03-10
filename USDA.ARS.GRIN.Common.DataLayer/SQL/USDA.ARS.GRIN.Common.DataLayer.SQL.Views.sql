USE [training]
GO
/****** Object:  View [dbo].[vw_gringlobal_weborders]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_weborders]
GO
/****** Object:  View [dbo].[vw_gringlobal_web_order_requests]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_web_order_requests]
GO
/****** Object:  View [dbo].[vw_gringlobal_web_cooperators]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_web_cooperators]
GO
/****** Object:  View [dbo].[vw_gringlobal_sys_users]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_sys_users]
GO
/****** Object:  View [dbo].[vw_gringlobal_sys_tables]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_sys_tables]
GO
/****** Object:  View [dbo].[vw_gringlobal_sys_table]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_sys_table]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_subcontinents]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_geography_subcontinents]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_states]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_geography_states]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_continents]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_geography_continents]
GO
/****** Object:  View [dbo].[vw_gringlobal_folders]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_folders]
GO
/****** Object:  View [dbo].[vw_gringlobal_cooperator_names]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_cooperator_names]
GO
/****** Object:  View [dbo].[vw_gringlobal_app_user_items]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_app_user_items]
GO
/****** Object:  View [dbo].[vw_gringlobal_organizations]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_organizations]
GO
/****** Object:  View [dbo].[vw_gringlobal_cooperators]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_cooperators]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_adm2_types]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_geography_adm2_types]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_adm1_types]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_geography_adm1_types]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_geography]
GO
/****** Object:  View [dbo].[vw_gringlobal_codevalues]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_codevalues]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_countries]    Script Date: 1/4/2022 12:05:14 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_gringlobal_geography_countries]
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_countries]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_gringlobal_geography_countries]
AS
select distinct 
	
	g.country_code as countrycode, 
	cvl.title as countryname 
from 
	geography g 
	join code_value cv 
		on g.country_code = cv.value
	join code_value_lang cvl 
		on cv.code_value_id = cvl.code_value_id 
where 
	cv.group_name = 'GEOGRAPHY_COUNTRY_CODE'
	and cvl.sys_lang_id = 1
GO
/****** Object:  View [dbo].[vw_gringlobal_codevalues]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vw_gringlobal_codevalues]
AS
SELECT
	cv.code_value_id As CodeValueID,
	cvl.code_value_lang_id AS CodeValueLangID,
	cv.group_name AS GroupName,
	cv.value As Code,
	cvl.title AS CodeTitle,
	cvl.description AS CodeDescription,
	cv.created_date AS CreatedDate,
	cv.created_by AS CreatedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = cv.created_by) AS CreatedByCooperatorName,
	cv.modified_date AS ModifiedDate,
	cv.modified_by AS ModifiedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = cv.modified_by) AS ModifiedByCooperatorName
FROM
	code_value cv
JOIN
	code_value_lang cvl
ON
	cv.code_value_id = cvl.code_value_id
AND
	cvl.sys_lang_id = 1
GO
/****** Object:  View [dbo].[vw_gringlobal_geography]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--SELECT * FROM vw_gringlobal_geography_continents
--ORDER BY continent_name

--SELECT * FROM vw_gringlobal_geography_subcontinents

CREATE VIEW [dbo].[vw_gringlobal_geography]
AS
SELECT
	grm.region_id,
	g.geography_id,
	LTRIM(RTRIM(COALESCE(cvl.title, g.country_code) + 
               CASE COALESCE(CONVERT(NVARCHAR, g.adm1), '') WHEN '' THEN '' ELSE ', ' + g.adm1 END +
               CASE COALESCE(CONVERT(NVARCHAR, g.adm2), '') WHEN '' THEN '' ELSE ', ' + g.adm2 END)) AS geography_text,
	r.continent,
	r.subcontinent,
	(SELECT countryname FROM vw_gringlobal_geography_countries WHERE LTRIM(RTRIM(countrycode)) = LTRIM(RTRIM(g.country_code))) AS country_name,
	g.adm1,
	g.adm2,
	g.country_code,
	g.adm1_type_code,
	(SELECT Code FROM vw_gringlobal_codevalues WHERE GroupName = 'GEOGRAPHY_ADMIN1_TYPE' AND LTRIM(RTRIM((adm1_type_code))) = LTRIM(RTRIM(value))) AS ADMIN1_TYPE,
	g.adm2_type_code,
	(SELECT Code FROM vw_gringlobal_codevalues WHERE GroupName = 'GEOGRAPHY_ADMIN2_TYPE' AND LTRIM(RTRIM((adm2_type_code))) = LTRIM(RTRIM(value))) AS ADMIN2_TYPE
FROM 
	geography g
LEFT JOIN
	geography_region_map grm
ON
	g.geography_id = grm.geography_id
LEFT JOIN
	region r
ON
	grm.region_id = r.region_id
 LEFT JOIN code_value cv ON g.country_code = cv.value AND cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' 
    LEFT JOIN code_value_lang cvl ON  cv.code_value_id = cvl.code_value_id AND  cvl.sys_lang_id = 1

GO
/****** Object:  View [dbo].[vw_gringlobal_geography_adm1_types]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_gringlobal_geography_adm1_types]
AS
SELECT 
	CONVERT(INT, LTRIM(RTRIM(Code))) AS value,
	CodeDescription
FROM vw_gringlobal_codevalues
WHERE GroupName = 'GEOGRAPHY_ADMIN1_TYPE'
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_adm2_types]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_gringlobal_geography_adm2_types]
AS
SELECT 
	LTRIM(RTRIM(Code)) AS value,
	CodeDescription
FROM 
	vw_gringlobal_codevalues
WHERE GroupName LIKE '%GEOGRAPHY_ADMIN2_TYPE%'
GO
/****** Object:  View [dbo].[vw_gringlobal_cooperators]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vw_gringlobal_cooperators]
AS
SELECT
	c.cooperator_id AS ID,
	wc.web_cooperator_id AS WebCooperatorID,
	wu.web_user_id AS WebUserID,
	c.first_name AS FirstName,
	c.last_name AS LastName,
	c.email AS EmailAddress,
	c.organization AS Organization,
	c.organization_abbrev AS OrganizationAbbrev,
	c.site_id AS SiteID,
	(SELECT site_long_name FROM site WHERE site_id = c.site_id) AS SiteName,
	su.sys_user_id AS SysUserID,
	su.user_name AS UserName,
	c.address_line1 AS Address1,
	c.address_line2 AS Address2,
	c.address_line3 AS Address3,
	c.city AS City,
	vgg.geography_text AS State,
	STUFF(COALESCE(' ' + c.address_line1, '') + '<br>' + 
      Coalesce(' ' + c.address_line2, '') + '<br>' +
      Coalesce(' ' + c.city, '') + '<br>' +
      Coalesce(' ' + vgg.geography_text, '') + 
      COALESCE('-' + NULLIF(c.postal_index, ''), ''), 1, 1, '') AS DisplayAddress
FROM
	cooperator c
JOIN
	vw_gringlobal_geography vgg
ON
	c.geography_id = vgg.geography_id
LEFT JOIN
	sys_user su
ON
	c.cooperator_id = su.cooperator_id
LEFT JOIN
	web_cooperator wc
ON
	c.web_cooperator_id = wc.web_cooperator_id
LEFT JOIN
	web_user wu
ON
	wu.web_cooperator_id = wc.web_cooperator_id
WHERE
	c.first_name IS NOT NULL AND c.last_name IS NOT NULL
AND
	LEN(LTRIM(RTRIM(c.first_name))) > 0 
GO
/****** Object:  View [dbo].[vw_gringlobal_organizations]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vw_gringlobal_organizations]
AS
SELECT
	c.cooperator_id AS ID,
	c.first_name AS FirstName,
	c.last_name AS LastName,
	c.email AS EmailAddress,
	c.organization AS Organization,
	c.site_id AS SiteID,
	(SELECT site_long_name FROM site WHERE site_id = c.site_id) AS SiteName,
	su.sys_user_id AS SysUserID,
	su.user_name AS UserName,
	c.address_line1 AS Address1,
	c.address_line2 AS Address2,
	c.address_line3 AS Address3,
	c.city AS City,
	vgg.geography_text AS State,
	STUFF(COALESCE(' ' + c.address_line1, '') + '<br>' + 
      Coalesce(' ' + c.address_line2, '') + '<br>' +
      Coalesce(' ' + c.city, '') + '<br>' +
      Coalesce(' ' + vgg.geography_text, '') + 
      COALESCE('-' + NULLIF(c.postal_index, ''), ''), 1, 1, '') AS DisplayAddress
FROM
	cooperator c
JOIN
	vw_gringlobal_geography vgg
ON
	c.geography_id = vgg.geography_id
LEFT JOIN
	sys_user su
ON
	c.cooperator_id = su.cooperator_id
WHERE
	(c.first_name IS NULL OR LEN(LTRIM(RTRIM(c.first_name))) = 0)
AND
	(c.last_name IS NULL OR LEN(LTRIM(RTRIM(c.first_name))) = 0)
GO
/****** Object:  View [dbo].[vw_gringlobal_app_user_items]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[vw_gringlobal_app_user_items]
AS
SELECT 
	auil.app_user_item_list_id AS ID,
    auil.app_user_item_folder_id AS FolderID,
    auil.cooperator_id AS CooperatorID,
    auil.tab_name AS TabName,
    auil.list_name AS FolderTitle,
    auil.id_number AS EntityID,
    auil.id_type AS EntityIDType,
	CASE auil.id_type
		WHEN 'CITATION_ID' THEN 'Citation'
		WHEN 'CROP_TRAIT_ID' THEN 'CWRTrait'
		WHEN 'LITERATURE_ID' THEN 'Literature'
		WHEN 'TAXONOMY_CWR_MAP_ID' THEN 'CWRMap'
		WHEN 'TAXONOMY_FAMILY_ID' THEN 'Family'
		WHEN 'TAXONOMY_GENUS_ID' THEN 'Genus'
		WHEN 'TAXONOMY_GEOGRAPHY_MAP_ID' THEN 'GeographyMap'
		WHEN 'TAXONOMY_REGULATION_ID' THEN 'Regulation'
		WHEN 'TAXONOMY_REGULATION_MAP_ID' THEN 'RegulationMap'
		WHEN 'TAXONOMY_SPECIES_ID' THEN 'Species'
		WHEN 'TAXONOMY_USE_ID' THEN 'EconomicUse'
		END AS EditURL,
    auil.sort_order As SortOrder,
    auil.title AS EntityTitle,
    auil.description AS Description,
    auil.properties AS Properties,
    auil.created_date AS CreatedDate,
	auil.created_by AS CreatedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator where cooperator_id = auil.created_by) AS CreatedByCooperatorName,
	auil.modified_date AS ModifiedDate,
	auil.modified_by AS ModifiedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = auil.modified_by) AS ModifiedByCooperatorName
FROM 
	app_user_item_list auil
WHERE
	id_type != 'FOLDER'
GO
/****** Object:  View [dbo].[vw_gringlobal_cooperator_names]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_gringlobal_cooperator_names]
AS
SELECT DISTINCT first_name + ' ' + last_name As cooperator_name FROM cooperator
GO
/****** Object:  View [dbo].[vw_gringlobal_folders]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[vw_gringlobal_folders]
AS
SELECT  
	auif.app_user_item_folder_id AS ID,
	auif.folder_name AS Title, 
	auif.folder_type AS FolderType,
	auif.description AS Description,
	auif.is_shared AS IsShared,
	auif.is_favorite AS IsFavorite,
	auif.created_by AS CreatedByCooperatorID,
	first_name + ' ' + last_name AS CooperatorName,
	auif.created_date AS CreatedDate
FROM 
	app_user_item_folder auif
JOIN
	cooperator c
ON
	auif.created_by = c.cooperator_id
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_continents]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_gringlobal_geography_continents]
AS
SELECT  
	region_id as id, 
	continent as continent_name
from 
	region 
where 
	subcontinent is null 
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_states]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_gringlobal_geography_states]
AS
select 
distinct 
	geography_id as gid, 
	adm1 as stateName 
from 
	geography 
where 
	country_code = 'USA' 
	and adm2 is null and adm3 is null and adm4 is null
GO
/****** Object:  View [dbo].[vw_gringlobal_geography_subcontinents]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_gringlobal_geography_subcontinents]
AS
select 
	region_id, 
	subcontinent 
from 
	region 
 where 
	continent in 
	(select continent from region where region_id = 100) 
	and subcontinent is not null
GO
/****** Object:  View [dbo].[vw_gringlobal_sys_table]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vw_gringlobal_sys_table]
AS
SELECT 
	st.sys_table_id,
	st.table_name,
	stl.title
FROM 
	sys_table st
LEFT OUTER JOIN
	sys_table_lang stl
ON
	st.sys_table_id = stl.sys_table_id
WHERE
	st.table_name LIKE 'taxonomy_%'
GO
/****** Object:  View [dbo].[vw_gringlobal_sys_tables]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vw_gringlobal_sys_tables]
AS
SELECT 
	st.sys_table_id,
	st.table_name,
	stl.title,
	stf.field_name
FROM 
	sys_table st
LEFT OUTER JOIN
	sys_table_lang stl
ON
	st.sys_table_id = stl.sys_table_id
JOIN
	sys_table_field stf
ON
	st.sys_table_id = stf.sys_table_id
WHERE
	st.table_name LIKE 'taxonomy_%'
GO
/****** Object:  View [dbo].[vw_gringlobal_sys_users]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vw_gringlobal_sys_users]
AS
	SELECT  
		su.sys_user_id AS ID,
		su.user_name AS UserName,
		su.password AS Password,
		su.is_enabled AS IsEnabled,
		c.cooperator_id AS CooperatorID,
		c.last_name AS LastName,
		c.first_name AS FirstName,
		LTRIM(RTRIM(COALESCE(c.last_name, '') + ', ' + COALESCE(c.first_name, '') )) AS FullName,
		c.email AS EmailAddress,
		c.organization AS Organization,
		c.address_line1 + ' ' + ISNULL(c.address_line2,'') + ' ' + ISNULL(c.address_line3,'') AS Address,
		CASE WHEN c.cooperator_id = c.current_cooperator_id THEN 'Y' ELSE 'N' END AS IsCurrentAddress,
		s.site_short_name AS SiteShortName, 
		c.city AS City,
		c.geography_id AS Geography,
		c.postal_index AS PostalCode,
		(SELECT virtual_path FROM cooperator_attach WHERE attach_cooperator_id = c.cooperator_id) AS CooperatorDefaultImageURL,
		su.created_date AS CreatedDate,
		su.created_by AS CreatedByCooperatorID
	FROM
		cooperator c
	LEFT JOIN
		sys_user su
	ON
		c.cooperator_id = su.cooperator_id
	JOIN	
		site s
	ON
		c.site_id = s.site_id
GO
/****** Object:  View [dbo].[vw_gringlobal_web_cooperators]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW
	[dbo].[vw_gringlobal_web_cooperators]
AS
SELECT
	wc.web_cooperator_id AS ID,
	wu.web_user_id AS WebUserID,
	LTRIM(RTRIM(wu.user_name)) AS WebUserName,
	LTRIM(RTRIM(wc.last_name)) AS LastName,
	LTRIM(RTRIM(wc.first_name)) AS FirstName,
	wc.job AS JobTitle,
	LTRIM(RTRIM(wc.organization)) AS Organization,
	wc.organization_code AS OrganizationAbbreviation,
	wc.address_line1 AS Address1,
	wc.address_line2 AS Address2,
	wc.address_line3 AS Aaddress3,
	wc.city AS City,
	(SELECT adm1 FROM geography WHERE geography_id = wc.geography_id) AS WebCooperatorAddressState,
	wc.postal_index AS PostalIndex,
	wc.created_date AS CreatedDate
FROM 
	web_cooperator wc
LEFT JOIN
	web_user wu
ON
	wu.web_cooperator_id = wc.web_cooperator_id
GO
/****** Object:  View [dbo].[vw_gringlobal_web_order_requests]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW 
	[dbo].[vw_gringlobal_web_order_requests]
AS
	
SELECT 
		wor.web_order_request_id AS ID,
		(SELECT COUNT(*) FROM order_request WHERE web_order_request_id = wor.web_order_request_id) AS OrderRequestCount,
		wor.web_cooperator_id As WebCooperatorID
		,wc.last_name AS WebCooperatorLastName
		,wc.title AS WebCooperatorTitle
		,wc.first_name AS WebCooperatorFirstName
		,(wc.first_name + ' ' + wc.last_name) AS WebCooperatorFullName
		,wc.primary_phone AS WebCooperatorPrimaryPhone
		,wc.email AS WebCooperatorEmail
		,wc.organization AS WebCooperatorOrganization 
		,wc.address_line1 AS WebCooperatorAddress1
		,wc.address_line2 AS WebCooperatorAddress2
		,wc.address_line3 AS WebCooperatorAddress3
		,wc.city AS WebCooperatorAddressCity
		,wc.postal_index AS WebCooperatorAddressPostalIndex,
		(SELECT adm1 FROM geography WHERE geography_id = wc.geography_id) AS WebCooperatorAddressState
		,wora.address_line1 AS ShippingAddress1
		,wora.address_line2 AS ShippingAddress2
		,wora.address_line3 AS ShippingAddress3
		,wora.city AS ShippingAddressCity
		,wora.postal_index AS ShippingAddressPostalIndex,
		(SELECT adm1 FROM geography WHERE geography_id = wora.geography_id) AS ShippingAddressState
		,DATEADD(hour,-5,ordered_date) AS OrderDate
		,wor.intended_use_code AS IntendedUseCode
		,wor.intended_use_note AS IntendedUseNote
		,wor.status_code AS StatusCode
		,wor.note AS Note
		,wor.special_instruction AS SpecialInstruction
		,(SELECT COUNT(*) FROM web_order_request_item WHERE web_order_request_id = wor.web_order_request_id) AS TotalItems
		,wor.created_date AS CreatedDate
		,wor.created_by AS CreatedByWebUserID,
		--(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = wor.created_by) AS CreatedByCooperatorName,
		wor.modified_date AS ModifiedByDate,
		wor.modified_by AS ModifiedByWebUserID,
		--(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = wor.modified_by) AS modified_by_name,
		wor.owned_date AS OwnedDate,
		wor.owned_by AS OwnedByWebUserID,
		(SELECT first_name + ' ' + last_name FROM web_cooperator wc1
		JOIN
			web_user wu1
		ON
			wc1.web_cooperator_id = wu1.web_cooperator_id
		AND
			wu1.web_user_id = wor.owned_by) AS OwnedByWebCooperatorName
	FROM
		web_order_request wor
	JOIN 
		web_cooperator wc 
	ON 
		wc.web_cooperator_id = wor.web_cooperator_id
	LEFT OUTER JOIN 
		web_order_request_address wora 
	ON 
		wora.web_order_request_id = wor.web_order_request_id
	JOIN
		geography g
	ON
		wc.geography_id = g.geography_id
GO
/****** Object:  View [dbo].[vw_gringlobal_weborders]    Script Date: 1/4/2022 12:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW 
	[dbo].[vw_gringlobal_weborders]
AS
	
SELECT 
		wor.web_order_request_id AS ID,
		(SELECT COUNT(*) FROM order_request WHERE web_order_request_id = wor.web_order_request_id) AS OrderRequestCount,
		wor.web_cooperator_id As WebCooperatorID
		,wc.last_name AS WebCooperatorLastName
		,wc.title AS WebCooperatorTitle
		,wc.first_name AS WebCooperatorFirstName
		,(wc.first_name + ' ' + wc.last_name) AS WebCooperatorFullName
		,wc.primary_phone AS WebCooperatorPrimaryPhone
		,wc.email AS WebCooperatorEmail
		,wc.organization AS WebCooperatorOrganization 
		,wc.address_line1 AS WebCooperatorAddress1
		,wc.address_line2 AS WebCooperatorAddress2
		,wc.address_line3 AS WebCooperatorAddress3
		,wc.city AS WebCooperatorAddressCity
		,wc.postal_index AS WebCooperatorAddressPostalIndex,
		(SELECT adm1 FROM geography WHERE geography_id = wc.geography_id) AS WebCooperatorAddressState
		,wora.address_line1 AS ShippingAddress1
		,wora.address_line2 AS ShippingAddress2
		,wora.address_line3 AS ShippingAddress3
		,wora.city AS ShippingAddressCity
		,wora.postal_index AS ShippingAddressPostalIndex,
		(SELECT adm1 FROM geography WHERE geography_id = wora.geography_id) AS ShippingAddressState
		,wor.ordered_date AS OrderDate
		,wor.intended_use_code AS IntendedUseCode
		,wor.intended_use_note AS IntendedUseNote
		,wor.status_code AS StatusCode
		,wor.note AS Note
		,wor.special_instruction AS SpecialInstruction
		,wor.created_date AS CreatedDate
		,wor.created_by AS CreatedByWebUserID,
		--(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = wor.created_by) AS CreatedByCooperatorName,
		wor.modified_date AS ModifiedByDate,
		wor.modified_by AS ModifiedByWebUserID,
		--(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = wor.modified_by) AS modified_by_name,
		wor.owned_date AS OwnedDate,
		wor.owned_by AS OwnedByWebUserID,
		(SELECT first_name + ' ' + last_name FROM web_cooperator wc1
		JOIN
			web_user wu1
		ON
			wc1.web_cooperator_id = wu1.web_cooperator_id
		AND
			wu1.web_user_id = wor.owned_by) AS OwnedByWebCooperatorName
	FROM
		web_order_request wor
	JOIN 
		web_cooperator wc 
	ON 
		wc.web_cooperator_id = wor.web_cooperator_id
	LEFT OUTER JOIN 
		web_order_request_address wora 
	ON 
		wora.web_order_request_id = wor.web_order_request_id
	JOIN
		geography g
	ON
		wc.geography_id = g.geography_id
GO
