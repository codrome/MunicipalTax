using MunicpalTax.Data;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Xml;

namespace MunicipaTax.DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Specify xml file to import (or use default importdata.xml): ");
            string fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = @".\importdata.xml";
            }

            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(fileName);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Unable to load file xml file {fileName}. Error {exc.Message}");
                return;
            }

            Console.WriteLine($"Starting import from file {fileName}");

            try
            {
                using (var db = new TaxRateContext())
                {
                    // cleanup existing entities before import
                    foreach (var entity in db.TaxRates)
                    {
                        db.TaxRates.Remove(entity);
                    }

                    var taxRateElems = document.GetElementsByTagName("TaxRate");

                    foreach (var taxRateElem in taxRateElems)
                    {
                        var taxRate = new TaxRate();

                        foreach (var childNode in (((XmlNode)taxRateElem).ChildNodes))
                        {
                            try
                            {
                                var xmlValue = ((XmlNode)childNode).InnerXml;

                                switch (((XmlNode)childNode).Name)
                                {
                                    case nameof(taxRate.Id):
                                        taxRate.Id = Guid.Parse(xmlValue);
                                        break;
                                    case nameof(taxRate.Name):
                                        taxRate.Name = xmlValue;
                                        break;
                                    case nameof(taxRate.MunicipalityName):
                                        taxRate.MunicipalityName = xmlValue;
                                        break;
                                    case nameof(taxRate.RateType):
                                        taxRate.RateType = int.Parse(xmlValue);
                                        break;
                                    case nameof(taxRate.StartDate):
                                        taxRate.StartDate = DateTime.Parse(xmlValue);
                                        break;
                                    case nameof(taxRate.EndDate):
                                        taxRate.EndDate = DateTime.Parse(xmlValue);
                                        break;
                                    case nameof(taxRate.Rate):
                                        taxRate.Rate = double.Parse(xmlValue);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            catch (Exception exc)
                            {
                                //TODO: improve by log provider
                                Console.WriteLine($"Unable to parse attribute {((XmlNode)childNode).Name} of xml node {((XmlNode)taxRateElem).InnerXml}. Error: {exc.Message}");
                            }
                        }

                        if (taxRate.Id == Guid.Empty)
                        {
                            taxRate.Id = Guid.NewGuid();
                        }

                        //TODO: check overlapping data ranges for same rate types

                        db.TaxRates.Add(taxRate);
                    }

                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException exc)
            {
                Console.WriteLine($"Unable to save changes, some entities are invalid: ");
                foreach (var entityError in exc.EntityValidationErrors)
                {
                    foreach (var error in entityError.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage} ");
                    }
                }

                Console.WriteLine($"Import aborted.");
                return;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Import aborted. Error: {exc.Message}");
                return;
            }

            using (var db = new TaxRateContext())
            {
                var count = db.TaxRates.Count();

                Console.WriteLine($"Successfully imported {count} entities.");
                //Console.WriteLine("Hit <Enter> to close.");
                //Console.ReadLine();
            }
        }
    }
}
