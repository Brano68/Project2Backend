using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCarRental.DTO;
using WebAppCarRental.Email;
using WebAppCarRental.JsonForBorrow;

namespace WebAppCarRental.Controllers
{
    [ApiController]
    [Route("borrow")]
    public class ControllerBorrowCar : ControllerBase
    {
        [HttpPost]
        [Route("car")]
        public async Task<ActionResult<BorrowDTO>> PostBorrow([FromBody] BorrowDTO borrowCar)
        {
            string login = borrowCar.Login;
            string token = borrowCar.Token;
            string plate = borrowCar.Plate;
            string from = borrowCar.From;
            string to = borrowCar.To;

            if (login == null || token == null || plate == null || from == null || to == null
                || login == "" || token == "" || plate == null || from == null || to == null) {
                return BadRequest("Wrong data!!!");
            }

            //overenie ci login sedi s tokenom a potom overenie ci SPZ auta je v databaze a potom overenie ci datum 
            //uz neni obsadeny
            using Data.ContosoUserContext contosoUserContext = new Data.ContosoUserContext();
            foreach (var row in contosoUserContext.Users)
            {
                if (row.Login == login) {
                    string tokenWithDateFromDatabase = row.Token;
                    var details = JObject.Parse(tokenWithDateFromDatabase);
                    string tokenWithoutDate = details["Token"].ToString();
                    //ak je token spravny
                    if(tokenWithoutDate == token)
                    {
                        //overime ci dana SPZ auta sa nachadza v tabulke a potom overime ci neni obsadeney datum pozicania
                        using Data.ContosoCarContext contosoCarContext = new Data.ContosoCarContext();
                        foreach (var row2 in contosoCarContext.Cars)
                        {
                            if(row2.Plate == plate)
                            {
                                DateTime dateTimeFrom = DateTime.Parse(from);
                                DateTime dateTimeTo = DateTime.Parse(to);
                                if(dateTimeFrom > dateTimeTo)
                                {
                                    return BadRequest("Date from is bigger than to!!!");
                                }
                                else
                                {
                                    string jsonReservations = row2.Reservations;
                                    //ak este auto neni pozicane tak Reservations je null
                                    if (jsonReservations == null)
                                    {
                                        //tu treba vytvrotit json v takom tvare
                                        //{"Borrows":[{"Login":"Brano","From":"6.4.2021","To":"7.4.2021"}]}
                                        //a poslat email
                                        List<Customer> customers = new List<Customer>();
                                        Customer customer = new Customer() 
                                        { 
                                            Login = login,
                                            From = from,
                                            To = to,
                                        };
                                        customers.Add(customer);
                                        //
                                        PatternBorrow patternBorrow = new PatternBorrow(customers);

                                        //string stringjson = JsonConvert.SerializeObject(customers, Formatting.Indented);
                                        string stringjson = JsonConvert.SerializeObject(patternBorrow);
                                        //ulozi do databazy
                                        int id = row2.Id;
                                        using (var context = new Data.ContosoCarContext())
                                        {
                                            var entity = context.Cars.FirstOrDefault(item => item.Id == id);
                                            entity.Reservations = stringjson;
                                            context.Cars.Update(entity);
                                            context.SaveChanges();
                                        }
                                        //poslanie emailu
                                        SendEmail sendEmail = new SendEmail();
                                        sendEmail.sendEmailToCustomer();
                                        return Ok("The car has been booked you should get an email");
                                    }
                                    //ak uz auto Reservation neni Null treba overit ci je volne!!!
                                    //a ak je volne poslat email
                                    else {
                                        string allReservations = row2.Reservations;
                                        //v premennej o je pole Borrows
                                        JObject o = JObject.Parse(allReservations);
                                        //cize tuna musim vybrat z pola datumi a porovnat ich
                                        //!!!
                                        //
                                        List<Customer> customers = new List<Customer>();
                                        
                                        JArray array = new JArray();
                                        array.Add(o["Borrows"]);
                                        foreach(var h in array)
                                        {
                                            foreach (var d in h) {
                                                //return Ok("Som v prvom objekte" + d["Login"]);
                                                string log = (string)d["Login"];
                                                DateTime dtFrom = DateTime.Parse((string)d["From"]);
                                                DateTime dtTo = DateTime.Parse((string)d["To"]);
                                                //return Ok(dtFrom + "  " + dtTo);
                                                //prejdem vsetke osoby
                                                Customer customer = new Customer()
                                                {
                                                    Login = log,
                                                    From = (string)d["From"],
                                                    To = (string)d["To"],
                                                };
                                                customers.Add(customer);
                                                //overime este vsetke datumy s tym co pride s Postmana
                                                if (dateTimeFrom > dtTo)
                                                { //dateTimeFrom < dtFrom && dateTimeTo < dtFrom
                                                    continue;
                                                }else if(dateTimeFrom < dtFrom && dateTimeTo < dtFrom) //dateTimeFrom > dtTo
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    return BadRequest("Date is taken already");
                                                }

                                            }
                                        }
                                        //ak prejdu foreach tak znamena ze vyhovuje 
                                        
                                        Customer customer1 = new Customer()
                                        {
                                            Login = login,
                                            From = from,
                                            To = to,
                                        };
                                        customers.Add(customer1);
                                        //vytvorime pole a ulozime do databazy update
                                        //
                                        //string stringjson = JsonConvert.SerializeObject(customers, Formatting.Indented);
                                        PatternBorrow patternBorrow = new PatternBorrow(customers);
                                        string stringjson = JsonConvert.SerializeObject(patternBorrow);
                                        //ulozi do databazy
                                        int id = row2.Id;
                                        using (var context = new Data.ContosoCarContext())
                                        {
                                            var entity = context.Cars.FirstOrDefault(item => item.Id == id);
                                            entity.Reservations = stringjson;
                                            context.Cars.Update(entity);
                                            context.SaveChanges();
                                        }
                                        //poslanie emailu
                                        SendEmail sendEmail = new SendEmail();
                                        sendEmail.sendEmailToCustomer();
                                        return Ok("The car has been booked you should get an email");
                                        //
                                        //return Ok("Uz neni null takze overovanie " + o["Borrows"]);
                                        
                                    }
                                    return Ok("Pouzil si spravny login aj token aj SPZ je ok uz len skontrolovat datum" + dateTimeFrom + " --- " + dateTimeTo);
                                }
                                //return Ok("Pouzil si spravny login aj token aj SPZ je ok uz len skontrolovat datum");
                            }
                        }
                            return Ok("Pouzil si spravny login aj token ale SPZ je zla");
                    }
                    //ak je token nespravny
                    return BadRequest("Wrong data!!!");
                }
            }

            return BadRequest("Wrong data!!!");
        }
    }
}
