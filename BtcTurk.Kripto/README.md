# BtcTurk.Kripto

BtcTurk.Kripto, BtcTurk kripto para borsası için geliştirilmiş resmi olmayan bir .NET kütüphanesidir. Bu kütüphane, BtcTurk API'sinin tüm public ve private uç noktalarını kullanarak kripto para alım-satım işlemlerini, hesap yönetimini ve pazar verilerini almanızı sağlar.

## 🚀 Özellikler

- **Tamamen Asenkron (Async/Await)** - Modern .NET async programlama modeli
- **Public API Desteği** - Kimlik doğrulama gerektirmeyen pazar verileri
- **Private API Desteği** - Hesap işlemleri ve trading için güvenli kimlik doğrulama
- **Güçlü Model Yapısı** - Tip güvenli veri modelleri
- **HMAC-SHA256 Güvenlik** - BtcTurk standartlarına uygun güvenlik
- **.NET 9.0** - En güncel .NET framework desteği

## 📦 Kurulum

### NuGet Paket Yöneticisi
```bash
Install-Package BtcTurk.Kripto -Version 1.0.0
```

### PackageReference
```xml
<PackageReference Include="BtcTurk.Kripto" Version="1.0.0" />
```

## 🔑 API Anahtarı Alımı

Private API uç noktalarını kullanmak için BtcTurk hesabınızdan API anahtarları oluşturmanız gerekir:

1. [BtcTurk Kripto](https://kripto.btcturk.com) hesabınıza giriş yapın
2. **Hesap > API Erişimi** sayfasına gidin
3. Gerekli izinleri seçerek yeni API anahtarı oluşturun

### Gerekli İzinler:
- **Toplam Varlık**: Bakiye sorgulama için
- **Al-Sat**: Emir verme/iptal etme için  
- **Hesap**: İşlem geçmişi için
- **WebSocket**: WebSocket işlemleri için

## 🛠️ Kullanım

### Temel Kullanım

```csharp
using BtcTurk.Kripto.Services;

// Public API için (API anahtarı gerektirmez)
var publicClient = new BtcTurkClient();

// Private API için (API anahtarları gereklidir)
var privateClient = new BtcTurkClient("API_KEY", "SECRET_KEY");
```

## 📊 Public API Kullanımı

Public API uç noktaları kimlik doğrulama gerektirmez ve herkes tarafından kullanılabilir.

### Borsa Bilgilerini Alma

```csharp
using BtcTurk.Kripto.Services;

var client = new BtcTurkClient();

// Tüm çiftlerin bilgilerini al
var exchangeInfo = await client.PublicService.GetExchangeInfoAsync();
if (exchangeInfo?.Success == true && exchangeInfo.Data != null)
{
    foreach (var symbol in exchangeInfo.Data.Symbols)
    {
        Console.WriteLine($"Çift: {symbol.Name}, Durum: {symbol.Status}");
        Console.WriteLine($"Minimum Miktar: {symbol.Filters?.MinQuantity}");
        Console.WriteLine($"Minimum Fiyat: {symbol.Filters?.MinPrice}");
    }
}
```

### Fiyat Bilgilerini Alma (Ticker)

```csharp
// Tüm çiftlerin fiyat bilgilerini al
var allTickers = await client.PublicService.GetTickersAsync();
if (allTickers?.Success == true && allTickers.Data != null)
{
    foreach (var ticker in allTickers.Data)
    {
        Console.WriteLine($"{ticker.Pair}: {ticker.Last} TL");
        Console.WriteLine($"24s Değişim: %{ticker.DailyPercent:F2}");
        Console.WriteLine($"Hacim: {ticker.Volume:F2}");
    }
}

// Belirli bir çiftin fiyat bilgilerini al
var btcTicker = await client.PublicService.GetTickersAsync("BTCTRY");
if (btcTicker?.Success == true && btcTicker.Data?.Length > 0)
{
    var ticker = btcTicker.Data[0];
    Console.WriteLine($"BTC/TRY Son Fiyat: {ticker.Last} TL");
    Console.WriteLine($"En Yüksek: {ticker.High} TL");
    Console.WriteLine($"En Düşük: {ticker.Low} TL");
}

// Para birimine göre çiftleri al
var usdtPairs = await client.PublicService.GetTickersByCurrencyAsync("USDT");
```

### Emir Defteri (Order Book)

```csharp
// BTCTRY çiftinin emir defterini al
var orderBook = await client.PublicService.GetOrderBookAsync("BTCTRY", limit: 20);
if (orderBook?.Success == true && orderBook.Data != null)
{
    Console.WriteLine("ALIM EMİRLERİ:");
    foreach (var bid in orderBook.Data.Bids)
    {
        Console.WriteLine($"Fiyat: {bid[0]}, Miktar: {bid[1]}");
    }
    
    Console.WriteLine("\nSATIM EMİRLERİ:");
    foreach (var ask in orderBook.Data.Asks)
    {
        Console.WriteLine($"Fiyat: {ask[0]}, Miktar: {ask[1]}");
    }
}
```

### Son İşlemler (Trades)

```csharp
// BTCTRY çiftinin son işlemlerini al
var trades = await client.PublicService.GetTradesAsync("BTCTRY", last: 10);
if (trades?.Success == true && trades.Data != null)
{
    foreach (var trade in trades.Data)
    {
        Console.WriteLine($"Fiyat: {trade.Price}, Miktar: {trade.Amount}");
        Console.WriteLine($"Tarih: {trade.DateTime}, Tip: {trade.Side}");
    }
}
```

## 🔐 Private API Kullanımı

Private API uç noktaları, hesap işlemleri ve trading için kullanılır. API anahtarları gereklidir.

### Hesap Bakiyesi

```csharp
using BtcTurk.Kripto.Services;

var client = new BtcTurkClient("API_KEY", "SECRET_KEY");

// Hesap bakiyelerini al
var balances = await client.PrivateService.GetBalancesAsync();
if (balances?.Success == true && balances.Data != null)
{
    foreach (var balance in balances.Data)
    {
        if (balance.Balance > 0)
        {
            Console.WriteLine($"{balance.Asset}: {balance.Balance}");
            Console.WriteLine($"Kullanılabilir: {balance.Free}");
            Console.WriteLine($"Kilitli: {balance.Locked}");
        }
    }
}
```

### Emir Verme

```csharp
// Market alım emri (piyasa fiyatından al)
var buyOrder = await client.PrivateService.SubmitOrderAsync(
    symbol: "BTCTRY",
    price: 0, // Market emirlerde fiyat 0 olmalı
    quantity: 0.001m, // 0.001 BTC
    side: "buy", // Alım emri
    type: "market" // Piyasa emri
);

if (buyOrder?.Success == true)
{
    Console.WriteLine($"Alım emri başarıyla verildi. Emir ID: {buyOrder.Data?.Id}");
}

// Limit satım emri (belirli fiyattan sat)
var sellOrder = await client.PrivateService.SubmitOrderAsync(
    symbol: "BTCTRY",
    price: 2750000, // 2,750,000 TL
    quantity: 0.001m, // 0.001 BTC
    side: "sell", // Satım emri
    type: "limit" // Limit emir
);

if (sellOrder?.Success == true)
{
    Console.WriteLine($"Satım emri başarıyla verildi. Emir ID: {sellOrder.Data?.Id}");
}

// Stop-loss emri
var stopOrder = await client.PrivateService.SubmitOrderAsync(
    symbol: "BTCTRY",
    price: 2700000, // Tetikleme fiyatından sonra kullanılacak fiyat
    quantity: 0.001m,
    side: "sell",
    type: "stop_market",
    stopPrice: 2650000 // Stop tetikleme fiyatı
);
```

### Emir İptali

```csharp
// Belirli bir emri iptal et
long orderId = 12345678;
var cancelResult = await client.PrivateService.CancelOrderAsync(orderId);
if (cancelResult?.Success == true)
{
    Console.WriteLine("Emir başarıyla iptal edildi.");
}
```

### Emir Sorgulama

```csharp
// Belirli bir emrin detaylarını al
var orderDetails = await client.PrivateService.GetOrderAsync(12345678);
if (orderDetails?.Success == true && orderDetails.Data != null)
{
    var order = orderDetails.Data;
    Console.WriteLine($"Emir ID: {order.Id}");
    Console.WriteLine($"Çift: {order.PairSymbol}");
    Console.WriteLine($"Tip: {order.Type}");
    Console.WriteLine($"Durum: {order.Status}");
    Console.WriteLine($"Fiyat: {order.Price}");
    Console.WriteLine($"Miktar: {order.Quantity}");
}

// Açık emirleri listele
var openOrders = await client.PrivateService.GetOpenOrdersAsync("BTCTRY");
if (openOrders?.Success == true && openOrders.Data?.Asks != null)
{
    Console.WriteLine("AÇIK EMİRLER:");
    foreach (var order in openOrders.Data.Asks.Concat(openOrders.Data.Bids))
    {
        Console.WriteLine($"ID: {order.Id}, Fiyat: {order.Price}, Miktar: {order.Quantity}");
    }
}

// Tüm emirleri listele (sayfalama ile)
var allOrders = await client.PrivateService.GetAllOrdersAsync(
    symbol: "BTCTRY",
    limit: 100,
    startDate: DateTimeOffset.UtcNow.AddDays(-30), // Son 30 gün
    endDate: DateTimeOffset.UtcNow
);

if (allOrders?.Success == true && allOrders.Data != null)
{
    foreach (var order in allOrders.Data)
    {
        Console.WriteLine($"ID: {order.Id}, Durum: {order.Status}, Tarih: {order.DateTime}");
    }
}
```

## 🎯 Gelişmiş Kullanım Örnekleri

### Trading Bot Örneği

```csharp
public class SimpleTradingBot
{
    private readonly BtcTurkClient _client;
    
    public SimpleTradingBot(string apiKey, string secretKey)
    {
        _client = new BtcTurkClient(apiKey, secretKey);
    }
    
    public async Task RunAsync()
    {
        // BTC/TRY çiftinin güncel fiyatını al
        var ticker = await _client.PublicService.GetTickersAsync("BTCTRY");
        if (ticker?.Success != true || ticker.Data?.Length == 0)
            return;
            
        var currentPrice = ticker.Data[0].Last;
        Console.WriteLine($"Güncel BTC fiyatı: {currentPrice:N0} TL");
        
        // Hesap bakiyesini kontrol et
        var balances = await _client.PrivateService.GetBalancesAsync();
        if (balances?.Success != true) return;
        
        var tryBalance = balances.Data?.FirstOrDefault(x => x.Asset == "TRY");
        var btcBalance = balances.Data?.FirstOrDefault(x => x.Asset == "BTC");
        
        Console.WriteLine($"TRY Bakiye: {tryBalance?.Free:N2}");
        Console.WriteLine($"BTC Bakiye: {btcBalance?.Free:N8}");
        
        // Basit strateji: Fiyat belirli bir seviyenin altındaysa al
        decimal buyThreshold = 2600000; // 2.6M TL
        
        if (currentPrice < buyThreshold && tryBalance?.Free > 1000)
        {
            // 1000 TL'lik BTC al
            var buyAmount = 1000m / currentPrice;
            var buyOrder = await _client.PrivateService.SubmitOrderAsync(
                "BTCTRY", 0, buyAmount, "buy", "market"
            );
            
            if (buyOrder?.Success == true)
            {
                Console.WriteLine($"Alım emri verildi: {buyAmount:N8} BTC");
            }
        }
    }
}

// Kullanım
var bot = new SimpleTradingBot("API_KEY", "SECRET_KEY");
await bot.RunAsync();
```

### Fiyat Takip Sistemi

```csharp
public class PriceMonitor
{
    private readonly BtcTurkClient _client;
    private readonly Timer _timer;
    
    public PriceMonitor()
    {
        _client = new BtcTurkClient();
        _timer = new Timer(CheckPrices, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }
    
    private async void CheckPrices(object? state)
    {
        try
        {
            var tickers = await _client.PublicService.GetTickersAsync();
            if (tickers?.Success != true) return;
            
            var majorPairs = new[] { "BTCTRY", "ETHTRY", "AVXTRY", "ADATRY" };
            
            foreach (var pairName in majorPairs)
            {
                var ticker = tickers.Data?.FirstOrDefault(t => t.Pair == pairName);
                if (ticker != null)
                {
                    Console.WriteLine($"{ticker.Pair}: {ticker.Last:N2} TL " +
                                    $"({ticker.DailyPercent:+0.##;-0.##}%)");
                    
                    // Fiyat alarm sistemi
                    if (Math.Abs(ticker.DailyPercent) > 10)
                    {
                        Console.WriteLine($"⚠️ ALARM: {ticker.Pair} %{ticker.DailyPercent:F2} değişim!");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
}
```

## ⚠️ Hata Yönetimi

```csharp
try
{
    var balances = await client.PrivateService.GetBalancesAsync();
    
    if (balances?.Success == true)
    {
        // Başarılı işlem
        Console.WriteLine("Bakiyeler başarıyla alındı");
    }
    else
    {
        // API hatası
        Console.WriteLine($"API Hatası: {balances?.Code} - {balances?.Message}");
    }
}
catch (HttpRequestException ex)
{
    // Ağ bağlantı hatası
    Console.WriteLine($"Bağlantı hatası: {ex.Message}");
}
catch (ArgumentException ex)
{
    // Parametre hatası
    Console.WriteLine($"Parametre hatası: {ex.Message}");
}
catch (Exception ex)
{
    // Genel hata
    Console.WriteLine($"Beklenmeyen hata: {ex.Message}");
}
```

## 📋 API Uç Noktaları

### Public API (V2)
- ✅ `/api/v2/server/exchangeinfo` - Borsa bilgileri
- ✅ `/api/v2/ticker` - Fiyat bilgileri
- ✅ `/api/v2/ticker/currency` - Para birimi bazında fiyatlar
- ✅ `/api/v2/orderbook` - Emir defteri
- ✅ `/api/v2/trades` - Son işlemler

### Private API (V1)
- ✅ `/api/v1/users/balances` - Hesap bakiyesi
- ✅ `/api/v1/order` - Emir verme/iptal etme
- ✅ `/api/v1/order/{id}` - Emir sorgulama
- ✅ `/api/v1/allOrders` - Tüm emirler
- ✅ `/api/v1/openOrders` - Açık emirler

## 🔒 Güvenlik

- API anahtarlarınızı asla kaynak kodunda saklamayın
- Production ortamında environment variable veya güvenli yapılandırma kullanın
- API anahtarlarınızı düzenli olarak yenileyin
- Gereksiz izinleri API anahtarınıza vermeyin

```csharp
// ✅ Güvenli kullanım
var apiKey = Environment.GetEnvironmentVariable("BTCTURK_API_KEY");
var secretKey = Environment.GetEnvironmentVariable("BTCTURK_SECRET_KEY");
var client = new BtcTurkClient(apiKey!, secretKey!);

// ❌ Güvenli olmayan kullanım
var client = new BtcTurkClient("hardcoded_api_key", "hardcoded_secret");
```

## 🤝 Katkıda Bulunma

Bu proje açık kaynak kodludur ve katkılarınızı memnuniyetle karşılarız:

1. Bu repository'yi fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje özel lisans altındadır. Tüm hakları saklıdır.

## 📞 İletişim

- **Geliştirici**: Mehmet Emin KARADAĞ
- **GitHub**: [https://github.com/emin-karadag](https://github.com/emin-karadag)
- **BtcTurk API Dokümantasyonu**: [https://docs.btcturk.com/](https://docs.btcturk.com/)

## 💰 Bağış

Bu projeyi beğendiyseniz ve geliştirilmesine katkıda bulunmak istiyorsanız, kripto para bağışı yapabilirsiniz:

**Tüm Ağlar İçin Cüzdan Adresi:**
```
0x21bc1e50042708a30275c151e43f7b1c1be99f2f
```

Desteklenen tokenlar: Tüm ERC-20/BEP-20 tokenları

## ⚡ Performans İpuçları

- `HttpClient` instance'ını yeniden kullanın (Dependency Injection ile)
- Gereksiz API çağrılarından kaçının (rate limit)
- Async/await pattern'ini doğru kullanın
- Exception handling implementasyonu yapın

## 🔄 Sürüm Geçmişi

- **v1.0.0** - İlk sürüm
  - Public API desteği
  - Private API desteği  
  - HMAC-SHA256 güvenlik
  - Async/await desteği

---

💡 **Not**: Bu kütüphane BtcTurk tarafından resmi olarak desteklenmemektedir. Kendi sorumluluğunuzda kullanın. 