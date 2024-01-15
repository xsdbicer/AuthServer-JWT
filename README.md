Bu proje, C# .NET kullanarak JWT (JSON Web Token) token'larını oluşturmayı ve doğrulamayı gösteren bir örnektir. Proje, aynı zamanda [Udemy üzerindeki "ASP.NET Core API + Token Bazlı Kimlik Doğrulama (JWT)"](<https://www.udemy.com/course/aspnet-core-api-token-bazli-kimlik-dogrulama-jwt/>) eğitimine referans oluşturur.

## JWT Nedir?
JWT (JSON Web Token), iki taraflı (client-server) iletişimde kullanılan bir açık standarttır. JSON formatında veriyi içeren bir token'ı temsil eder ve bu token'lar, veriyi güvenli bir şekilde iki taraflı arasında taşımak için kullanılır.

<img width="626" alt="image" src="https://github.com/xsdbicer/AuthServer-JWT/assets/41507884/166ed303-dd5e-4a88-a4e9-98cdd1cc81c8">

Resim: https://jwt.io/

### JWT'nin Temel Bileşenleri:

- *Header (Başlık):* Token'ın türü (JWT) ve kullanılan algoritmanın bilgisi içerir.
  
- *Payload (İçerik):* Token'da taşınan verileri içerir. Kullanıcı adı, roller, token süresi gibi bilgiler bu bölümde bulunabilir.
  
- *Signature (İmza):* Token'ın doğruluğunu kontrol etmek için kullanılır. Header ve Payload'ın base64-encoded halleri ile bir secret key'in hash'lenmiş bir versiyonunun birleştirilmesiyle oluşur.

## JWT Ne İşe Yarar?
JWT, özellikle stateless (durumsuz) web uygulamalarında ve mikroservis mimarilerinde kimlik doğrulama ve yetkilendirme için kullanılır. İşte JWT'nin bazı avantajları:

- *Taşınabilirlik:* Token'lar, veriyi içerir ve bu veri taşınabilir olduğundan farklı platformlar arasında kullanılabilir.

- *Stateless (Durumsuz) Doğrulama:* Token'lar, sunucuda kullanıcı oturumu durumu (session state) gerektirmez. Bu, ölçeklenebilirliği artırır.

- *Çapraz Platform Desteği:* JWT, farklı platformlarda (örneğin, web uygulamaları, mobil uygulamalar) kullanılabilir.

## Access Token Nedir?
Access Token, bir kullanıcının bir kaynağa (örneğin, bir web hizmetine) erişimini doğrulayan bir kimlik belgesidir. Bu belge, kullanıcının kimliğini doğrulayan birçok bilgiyi içerir. Access Token'lar genellikle belirli bir süre boyunca geçerlidir ve süre sona erdiğinde yenilenmelidir.

## Refresh Token Nedir?
Refresh Token, Access Token'ın geçerliliğini yitirdiğinde, yeni bir Access Token almak için kullanılan bir belgedir. Refresh Token, genellikle daha uzun bir süre boyunca geçerlidir ve kullanıcının kimliğini doğrulayan bilgileri içerir.

Refresh Token, güvenli bir şekilde saklanmalıdır, çünkü bir kez ele geçirildiğinde uzun bir süre boyunca kullanılabilir.

### Kullanım Senaryosu
* Kullanıcı Girişi: Kullanıcı, kullanıcı adı ve şifre ile sisteme giriş yapar.
* Access Token Al: Kullanıcının kimliği doğrulandığında, sunucu bir Access Token ve bir Refresh Token oluşturur. Access Token, kullanıcının belirli kaynaklara erişimini sağlar.
* Access Token'ı Kullan: Kullanıcı, Access Token ile belirli kaynaklara erişir.
*Access Token Süresi Dolarsa: Eğer Access Token'ın süresi dolarsa, kullanıcı Refresh Token kullanarak yeni bir Access Token alır.

## Projenin işleyişi nasıl gerçekleşiyor?

<img width="890" alt="image" src="https://github.com/xsdbicer/AuthServer-JWT/assets/41507884/3557b7cf-fa67-4ae1-8830-c3bbccc5fde9">

1. Kullanıcı, web sitesi veya uygulamaya erişmek için ilk kez authserver ile iletişim kurar.
2. Authserver, kullanıcının kimliğini doğrular ve kullanıcıya bir access token ve refresh token verir.
3. Kullanıcı, access tokenı web sunucusuna göndererek web sitesi veya uygulamaya erişir.
4. Web sunucusu, access tokendaki kimliği doğrular ve kullanıcıya erişimi sağlar.
5. Access token süresi dolduğunda, kullanıcı yeni bir access token almak için refresh token ile authserver ile iletişim kurar.
6. Authserver, kullanıcıya yeni bir access token ve refresh token verir.
