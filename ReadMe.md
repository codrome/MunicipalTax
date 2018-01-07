1. Project MunicpalTax.Data - code first entity framework against mssqllocaldb (to setup database run Update-Database in PM console)
2. MunicipaTax.DataImport - console program to import xml file (default "importdata.xml") to database. Use EF directly.
3. MunicipalTax.Service - self hosted (Owin) executable service with WebAPI. Runs on http://localhost:9000.
  Swagger API on http://localhost:9000/swagger/ui/index
  TopShelf service hosting incomplete (runs, but service not installed/visible).
4. MunicipalTax.Consumer - console application compoment with tests and examples for to consume MunicipalTax.Service via REST endpoints.