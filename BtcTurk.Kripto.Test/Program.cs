using BtcTurk.Kripto.Services;

namespace BtcTurk.Kripto.Test;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 BtcTurk.Kripto Test Uygulaması");
        Console.WriteLine(new string('=', 50));

        // Menü göster
        while (true)
        {
            Console.WriteLine("\nLütfen test etmek istediğiniz API'yi seçin:");
            Console.WriteLine("1. Public API Testleri");
            Console.WriteLine("2. Private API Testleri");
            Console.WriteLine("3. Gelişmiş Örnekler");
            Console.WriteLine("0. Çıkış");
            Console.Write("Seçiminiz: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await TestPublicApiAsync();
                        break;
                    case "2":
                        await TestPrivateApiAsync();
                        break;
                    case "3":
                        await AdvancedExamplesAsync();
                        break;
                    case "0":
                        Console.WriteLine("Çıkış yapılıyor...");
                        return;
                    default:
                        Console.WriteLine("❌ Geçersiz seçim!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hata: {ex.Message}");
            }

            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static async Task TestPublicApiAsync()
    {
        Console.WriteLine("\n📊 PUBLIC API TESTLERİ");
        Console.WriteLine(new string('=', 30));

        var client = new BtcTurkClient();

        // 1. Borsa Bilgilerini Al
        Console.WriteLine("\n1️⃣ Borsa Bilgileri Alınıyor...");
        var exchangeInfo = await client.PublicService.GetExchangeInfoAsync();
        if (exchangeInfo?.Success == true && exchangeInfo.Data != null)
        {
            Console.WriteLine($"✅ Toplam {exchangeInfo.Data.Symbols?.Count} işlem çifti bulundu");

            // İlk 5 çifti göster
            var firstFive = exchangeInfo.Data.Symbols?.Take(5);
            if (firstFive != null)
            {
                foreach (var symbol in firstFive)
                {
                    Console.WriteLine($"   📈 {symbol.Name} - Durum: {symbol.Status}");
                    if (symbol.Filters?.Count > 0)
                    {
                        var filter = symbol.Filters[0];
                        Console.WriteLine($"      Min Miktar: {filter.MinAmount}");
                        Console.WriteLine($"      Min Fiyat: {filter.MinPrice}");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("❌ Borsa bilgileri alınamadı");
        }

        // 2. Tüm Fiyat Bilgilerini Al
        Console.WriteLine("\n2️⃣ Fiyat Bilgileri Alınıyor...");
        var allTickers = await client.PublicService.GetTickersAsync();
        if (allTickers?.Success == true && allTickers.Data != null)
        {
            Console.WriteLine($"✅ {allTickers.Data.Length} çiftin fiyat bilgisi alındı");

            // Popüler çiftleri göster
            var popularPairs = new[] { "BTCTRY", "ETHTRY", "USDTTRY", "AVXTRY" };
            foreach (var pairName in popularPairs)
            {
                var ticker = allTickers.Data.FirstOrDefault(t => t.Pair == pairName);
                if (ticker != null)
                {
                    Console.WriteLine($"   💰 {ticker.Pair}: {ticker.Last:N2} TL ({ticker.DailyPercent:+0.##;-0.##}%)");
                }
            }
        }

        // 3. Belirli Çift Fiyat Bilgisi
        Console.WriteLine("\n3️⃣ BTC/TRY Detaylı Bilgiler...");
        var btcTicker = await client.PublicService.GetTickersAsync("BTCTRY");
        if (btcTicker?.Success == true && btcTicker.Data?.Length > 0)
        {
            var ticker = btcTicker.Data[0];
            Console.WriteLine($"   📊 Son Fiyat: {ticker.Last:N0} TL");
            Console.WriteLine($"   📈 En Yüksek: {ticker.High:N0} TL");
            Console.WriteLine($"   📉 En Düşük: {ticker.Low:N0} TL");
            Console.WriteLine($"   💹 24s Hacim: {ticker.Volume:N2}");
            Console.WriteLine($"   🔄 24s Değişim: %{ticker.DailyPercent:F2}");
        }

        // 4. Emir Defteri
        Console.WriteLine("\n4️⃣ BTC/TRY Emir Defteri...");
        var orderBook = await client.PublicService.GetOrderBookAsync("BTCTRY", limit: 5);
        if (orderBook?.Success == true && orderBook.Data != null)
        {
            Console.WriteLine("   🟢 ALIM EMİRLERİ (İlk 5):");
            foreach (var bid in orderBook.Data.Bids.Take(5))
            {
                Console.WriteLine($"      Fiyat: {bid.Price:N0} TL, Miktar: {bid.Amount:N6} BTC");
            }

            Console.WriteLine("   🔴 SATIM EMİRLERİ (İlk 5):");
            foreach (var ask in orderBook.Data.Asks.Take(5))
            {
                Console.WriteLine($"      Fiyat: {ask.Price:N0} TL, Miktar: {ask.Amount:N6} BTC");
            }
        }

        // 5. Son İşlemler
        Console.WriteLine("\n5️⃣ BTC/TRY Son İşlemler...");
        var trades = await client.PublicService.GetTradesAsync("BTCTRY", last: 5);
        if (trades?.Success == true && trades.Data != null)
        {
            foreach (var trade in trades.Data)
            {
                var sideIcon = trade.Side == "buy" ? "🟢" : "🔴";
                Console.WriteLine($"   {sideIcon} {trade.Price:N0} TL - {trade.Amount:N6} BTC - {trade.DateTime:HH:mm:ss}");
            }
        }

        // 6. Para Birimi Bazında Çiftler
        Console.WriteLine("\n6️⃣ USDT Çiftleri...");
        var usdtPairs = await client.PublicService.GetTickersByCurrencyAsync("USDT");
        if (usdtPairs?.Success == true && usdtPairs.Data != null)
        {
            Console.WriteLine($"✅ {usdtPairs.Data.Length} USDT çifti bulundu");
            foreach (var ticker in usdtPairs.Data.Take(5))
            {
                Console.WriteLine($"   💵 {ticker.Pair}: ${ticker.Last:N4} ({ticker.DailyPercent:+0.##;-0.##}%)");
            }
        }
    }

    static async Task TestPrivateApiAsync()
    {
        Console.WriteLine("\n🔐 PRIVATE API TESTLERİ");
        Console.WriteLine(new string('=', 30));

        Console.WriteLine("⚠️  Private API testleri için API anahtarları gereklidir!");
        Console.WriteLine("Güvenlik nedeniyle bu örneklerde gerçek anahtarlar kullanılmamıştır.\n");

        // API anahtarlarını environment variable'dan al
        var apiKey = Environment.GetEnvironmentVariable("BTCTURK_API_KEY");
        var secretKey = Environment.GetEnvironmentVariable("BTCTURK_SECRET_KEY");

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey))
        {
            Console.WriteLine("💡 Environment variable'ları ayarlayarak test edebilirsiniz:");
            Console.WriteLine("   set BTCTURK_API_KEY=your_api_key");
            Console.WriteLine("   set BTCTURK_SECRET_KEY=your_secret_key");
            Console.WriteLine("\n📝 Örnek kod yapısı:");

            ShowPrivateApiExamples();
            return;
        }

        try
        {
            var client = new BtcTurkClient(apiKey, secretKey);

            // 1. Hesap Bakiyesi
            Console.WriteLine("1️⃣ Hesap Bakiyesi Kontrol Ediliyor...");
            var balances = await client.PrivateService.GetBalancesAsync();
            if (balances?.Success == true && balances.Data != null)
            {
                Console.WriteLine("✅ Bakiyeler başarıyla alındı:");
                foreach (var balance in balances.Data.Where(b => b.Balance > 0))
                {
                    Console.WriteLine($"   💰 {balance.Asset}: {balance.Balance}");
                    Console.WriteLine($"      Kullanılabilir: {balance.Free}");
                    Console.WriteLine($"      Kilitli: {balance.Locked}");
                }
            }

            // 2. Açık Emirler
            Console.WriteLine("\n2️⃣ Açık Emirler Kontrol Ediliyor...");
            var openOrders = await client.PrivateService.GetOpenOrdersAsync();
            if (openOrders?.Success == true && openOrders.Data != null)
            {
                var totalOrders = (openOrders.Data.Asks?.Length ?? 0) + (openOrders.Data.Bids?.Length ?? 0);
                Console.WriteLine($"✅ Toplam {totalOrders} açık emir bulundu");

                if (openOrders.Data.Asks?.Length > 0)
                {
                    Console.WriteLine("   🔴 Satım Emirleri:");
                    foreach (var order in openOrders.Data.Asks.Take(3))
                    {
                        Console.WriteLine($"      ID: {order.Id} - {order.Price:N2} TL - {order.Quantity:N6}");
                    }
                }

                if (openOrders.Data.Bids?.Length > 0)
                {
                    Console.WriteLine("   🟢 Alım Emirleri:");
                    foreach (var order in openOrders.Data.Bids.Take(3))
                    {
                        Console.WriteLine($"      ID: {order.Id} - {order.Price:N2} TL - {order.Quantity:N6}");
                    }
                }
            }

            // 3. Son Emirler
            Console.WriteLine("\n3️⃣ Son Emirler Getiriliyor...");
            var allOrders = await client.PrivateService.GetAllOrdersAsync(
                symbol: "BTCTRY",
                limit: 5
            );
            if (allOrders?.Success == true && allOrders.Data != null)
            {
                Console.WriteLine($"✅ Son {allOrders.Data.Length} emir:");
                foreach (var order in allOrders.Data)
                {
                    var statusIcon = order.Status switch
                    {
                        "Completed" => "✅",
                        "Cancelled" => "❌",
                        "Pending" => "⏳",
                        _ => "❓"
                    };
                    Console.WriteLine($"   {statusIcon} ID: {order.Id} - {order.Type} - {order.Status} - {order.DateTime:dd.MM.yyyy HH:mm}");
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Private API Hatası: {ex.Message}");
        }
    }

    static void ShowPrivateApiExamples()
    {
        Console.WriteLine(@"
// Hesap bakiyesi kontrolü
var client = new BtcTurkClient(""API_KEY"", ""SECRET_KEY"");
var balances = await client.PrivateService.GetBalancesAsync();

// Market alım emri
var buyOrder = await client.PrivateService.SubmitOrderAsync(
    symbol: ""BTCTRY"",
    price: 0,          // Market emirlerde 0
    quantity: 0.001m,  // 0.001 BTC
    side: ""buy"",
    type: ""market""
);

// Limit satım emri
var sellOrder = await client.PrivateService.SubmitOrderAsync(
    symbol: ""BTCTRY"",
    price: 2750000,    // 2,750,000 TL
    quantity: 0.001m,
    side: ""sell"",
    type: ""limit""
);

// Emir iptal
var cancelResult = await client.PrivateService.CancelOrderAsync(orderId);

// Açık emirleri listele
var openOrders = await client.PrivateService.GetOpenOrdersAsync(""BTCTRY"");");
    }

    static async Task AdvancedExamplesAsync()
    {
        Console.WriteLine("\n🎯 GELİŞMİŞ ÖRNEKLER");
        Console.WriteLine(new string('=', 30));

        // 1. Fiyat Alarmı Sistemi
        Console.WriteLine("\n1️⃣ Fiyat Alarm Sistemi Örneği");
        await PriceAlarmExample();

        // 2. Arbitraj Fırsatı Kontrolü
        Console.WriteLine("\n2️⃣ Basit Arbitraj Kontrolü");
        await ArbitrageOpportunityExample();

        // 3. Portfolio Değeri Hesaplama
        Console.WriteLine("\n3️⃣ Portfolio Değeri Hesaplama (Örnek)");
        await PortfolioValueExample();
    }

    static async Task PriceAlarmExample()
    {
        var client = new BtcTurkClient();

        // BTC için alarm seviyeleri
        decimal btcAlarmHigh = 2800000; // 2.8M TL
        decimal btcAlarmLow = 2400000;  // 2.4M TL

        var ticker = await client.PublicService.GetTickersAsync("BTCTRY");
        if (ticker?.Success == true && ticker.Data?.Length > 0)
        {
            var currentPrice = ticker.Data[0].Last;
            Console.WriteLine($"   📊 Güncel BTC Fiyatı: {currentPrice:N0} TL");
            Console.WriteLine($"   🔔 Yüksek Alarm: {btcAlarmHigh:N0} TL");
            Console.WriteLine($"   🔔 Düşük Alarm: {btcAlarmLow:N0} TL");

            if (currentPrice >= btcAlarmHigh)
            {
                Console.WriteLine("   🚨 YÜKSEK FİYAT ALARMI! Satış zamanı olabilir.");
            }
            else if (currentPrice <= btcAlarmLow)
            {
                Console.WriteLine("   🚨 DÜŞÜK FİYAT ALARMI! Alım fırsatı olabilir.");
            }
            else
            {
                Console.WriteLine("   ✅ Fiyat normal aralıkta.");
            }
        }
    }

    static async Task ArbitrageOpportunityExample()
    {
        var client = new BtcTurkClient();

        var tickers = await client.PublicService.GetTickersAsync();
        if (tickers?.Success == true && tickers.Data != null)
        {
            // TRY ve USDT çiftlerini karşılaştır
            var btcTry = tickers.Data.FirstOrDefault(t => t.Pair == "BTCTRY");
            var btcUsdt = tickers.Data.FirstOrDefault(t => t.Pair == "BTCUSDT");
            var usdtTry = tickers.Data.FirstOrDefault(t => t.Pair == "USDTTRY");

            if (btcTry != null && btcUsdt != null && usdtTry != null)
            {
                var btcPriceViaTry = btcTry.Last;
                var btcPriceViaUsdt = btcUsdt.Last * usdtTry.Last;
                var difference = Math.Abs(btcPriceViaTry - btcPriceViaUsdt);
                var differencePercent = (difference / btcPriceViaTry) * 100;

                Console.WriteLine($"   💰 BTC/TRY: {btcPriceViaTry:N0} TL");
                Console.WriteLine($"   💵 BTC/USDT: {btcUsdt.Last:N0} USDT");
                Console.WriteLine($"   💱 USDT/TRY: {usdtTry.Last:N2} TL");
                Console.WriteLine($"   🔄 BTC via USDT: {btcPriceViaUsdt:N0} TL");
                Console.WriteLine($"   📊 Fark: {difference:N0} TL (%{differencePercent:F2})");

                if (differencePercent > 1)
                {
                    Console.WriteLine("   🎯 Potansiyel arbitraj fırsatı tespit edildi!");
                }
            }
        }
    }

    static async Task PortfolioValueExample()
    {
        Console.WriteLine("   💼 Portfolio hesaplama için API anahtarları gerekli");
        Console.WriteLine("   📝 Örnek hesaplama mantığı:");

        var client = new BtcTurkClient();
        var tickers = await client.PublicService.GetTickersAsync();

        if (tickers?.Success == true)
        {
            // Örnek portfolio
            var examplePortfolio = new Dictionary<string, decimal>
            {
                { "BTC", 0.5m },
                { "ETH", 2.0m },
                { "AXS", 100.0m },
                { "TRY", 10000.0m }
            };

            decimal totalValue = 0;
            Console.WriteLine("   📊 Örnek Portfolio:");

            foreach (var asset in examplePortfolio)
            {
                if (asset.Key == "TRY")
                {
                    totalValue += asset.Value;
                    Console.WriteLine($"      {asset.Key}: {asset.Value:N2} TL");
                    continue;
                }

                var ticker = tickers.Data?.FirstOrDefault(t => t.Pair == $"{asset.Key}TRY");
                if (ticker != null)
                {
                    var assetValue = asset.Value * ticker.Last;
                    totalValue += assetValue;
                    Console.WriteLine($"      {asset.Key}: {asset.Value} × {ticker.Last:N2} = {assetValue:N2} TL");
                }
            }

            Console.WriteLine($"   💰 Toplam Portfolio Değeri: {totalValue:N2} TL");
        }
    }
}
