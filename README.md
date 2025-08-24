# WheezlyApp



Respuestas a enunciado:
2) What would you do if you had data that doesn’t change often but it’s used pretty much all
the time? Would it make a difference if you have more than one instance of your
application?
--Usaria cache, si la app tiene mas de una instancia usaria cache distribuido como redis por ejemplo

3) Analyze the following method and make changes to make it better. Explain your
changes.
public void UpdateCustomersBalanceByInvoices(List<Invoice> invoices)
{
   foreach (var invoice in invoices)
   {
      var customer = dbContext.Customers.SingleOrDefault(invoice.CustomerId.Value);

      customer.Balance -= invoice.Total;
      dbContext.SaveChanges();
   }
}

--lo cambiaria por:
public void UpdateCustomersBalanceByInvoices(List<Invoice> invoices)
{
   foreach (var invoice in invoices)
   {
      var customer = dbContext.Customers.SingleOrDefault(invoice.CustomerId.Value);

      customer.Balance -= invoice.Total;
      
   }
   dbContext.SaveChanges();
}
//para no hacer el set en la base de datos en cada iteracion


4 se agrega un OrderController para mostrar el codigo
