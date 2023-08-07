using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

const string url = "http://quotes.toscrape.com";
string path = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
string file = Path.Combine(path, "author-quotes.json");


IWebDriver driver = new ChromeDriver(); // create driver e.g chrome, firefox, safari etc.
driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15); // set event wait timeout

driver.Navigate().GoToUrl(url); // open url page in browser

/*** Perform login action ***/

driver.FindElement(By.LinkText("Login")).Click(); // login

var username = driver.FindElement(By.Id("username")); // find the inputs
var password = driver.FindElement(By.Id("password"));
username.SendKeys("admin"); // fill the inputs
password.SendKeys("1234");
driver.FindElement(By.CssSelector("input.btn-primary")).Click(); // submit

int page = 0;
List<AuthorQuote> aqlist = new();
while (true)
{
    var quotes = driver.FindElements(By.ClassName("text")); // find quotes
    var authors = driver.FindElements(By.ClassName("author"));  // find author
    Console.WriteLine("\nPage: {0}\n", ++page);
    foreach (var(quote, author) in Enumerable.Zip(quotes, authors))
    {
        Console.WriteLine($"{quote.Text} - {author.Text}");
        aqlist.Add(new AuthorQuote(quote.Text, author.Text));
    }

    try
    {
        driver.FindElement(By.PartialLinkText("Next")).Click(); // ?go to next page
    }
    catch(NoSuchElementException) { break; }
}

if (aqlist.Any()){ // save to json
    using var fs = File.Create(file);
    using var sw = new StreamWriter(fs);
    sw.WriteLine(System.Text.Json.JsonSerializer.Serialize(aqlist));
}

driver.FindElement(By.LinkText("Logout")).Click(); // logout

driver.Quit();

public record AuthorQuote(string Quote, string Author);