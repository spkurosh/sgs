CREATE VIEW vwDataSection AS
select 
	s.idSuburb, 
	s.Suburbname, 
	s.postalCode, 
	s.section,
	m.MunicipalityName,
	d.DistrictName
from suburbs s
INNER JOIN Municipalities m on s.idMunicipality = m.idMunicipality
INNER JOIN Districts d on d.IdDistrict = m.IdDistrict
