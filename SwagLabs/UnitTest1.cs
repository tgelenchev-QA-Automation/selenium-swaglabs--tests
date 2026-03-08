using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Threading.Tasks;


namespace SwagLabs
{
    public class Tests
    {

        IWebDriver driver;

        [SetUp]
        public void Setup()
        {

            driver = new ChromeDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string baseURL = "https://www.saucedemo.com";
            driver.Navigate().GoToUrl(baseURL);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void LogInWithValidCredention()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();


            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/inventory.html"));
        }

        [Test]
        public void LogInWithInvalidCredention()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("123");
            driver.FindElement(By.Id("login-button")).Click();

            string Error = "Epic sadface: Username and password do not match any user in this service";

            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/"));
            Assert.That(Error, Does.Contain("Epic sadface: Username and password do not match any user in this service"));
        }

        [Test]
        public void LogInLockedUser()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("locked_out_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            string lockedUser = "Epic sadface: Sorry, this user has been locked out.";

            Assert.That(lockedUser, Does.Contain("Epic sadface: Sorry, this user has been locked out."));
        }

        [Test]
        public void LogOut()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            driver.FindElement(By.Id("react-burger-menu-btn")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement logoutButton = wait.Until(
                ExpectedConditions.ElementToBeClickable(By.Id("logout_sidebar_link"))
            );

            logoutButton.Click();

            wait.Until(ExpectedConditions.UrlContains("saucedemo.com"));

            Assert.That(driver.Url, Does.Contain("saucedemo.com"));

        }

        [Test]
        public void AddToCart()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            driver.FindElement(By.Id("add-to-cart-sauce-labs-backpack")).Click();
            driver.FindElement(By.ClassName("shopping_cart_link")).Click();

            Assert.That(driver.Url, Is.EqualTo("https://www.saucedemo.com/cart.html"));

        }

        [Test]
        public void AddMultipleProducts()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            driver.FindElement(By.Id("add-to-cart-sauce-labs-backpack")).Click();
            driver.FindElement(By.Id("add-to-cart-sauce-labs-bike-light")).Click();
            driver.FindElement(By.Id("add-to-cart-sauce-labs-bolt-t-shirt")).Click();

            IWebElement cartBadge = driver.FindElement(By.ClassName("shopping_cart_badge"));
            Assert.That(cartBadge.Text, Is.EqualTo("3"));

            driver.FindElement(By.ClassName("shopping_cart_link")).Click();

            Assert.That(driver.Url, Does.Contain("cart.html"));

            var cartItems = driver.FindElements(By.ClassName("cart_item"));
            Assert.That(cartItems.Count, Is.EqualTo(3));
            Console.WriteLine(driver.Url);
        }

        [Test]
        public void RemoveFromCart()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            driver.FindElement(By.Id("add-to-cart-sauce-labs-backpack")).Click();
            driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            driver.FindElement(By.Id("remove-sauce-labs-backpack")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("remove-sauce-labs-backpack")));

            Assert.That(driver.FindElements(By.Id("remove-sauce-labs-backpack")).Count, Is.EqualTo(0));
        }


      

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
            driver.Quit();
        }
    }
}