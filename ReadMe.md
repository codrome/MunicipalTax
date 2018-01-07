# Project Title

Sample Municipal Tax service

## Project structure

1. Project MunicipalTax.Data - code first entity framework against mssqllocaldb (to setup database run Update-Database in PM console)
2. MunicipalTax.DataImport - console program to import xml file (default "importdata.xml") to database. Use EF directly.
3. MunicipalTax.Service - self hosted (Owin) executable service with WebAPI. Runs on http://localhost:9000.
  Swagger API on http://localhost:9000/swagger/ui/index
  TopShelf service hosting incomplete (runs, but service not installed/invisible).
4. MunicipalTax.Consumer - console application with tests and examples consuming MunicipalTax.Service via REST endpoints.