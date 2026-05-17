# KURS ISHI HISOBOTI

**Mavzu:** Kriptografik Algoritmlarni Vizuallashtirish (Standard ElGamal va EC-ElGamal)  
**Texnologiyalar:** C#, .NET 8, Blazor WebAssembly, MudBlazor  

---

## 1. Kirish
Zamonaviy axborot xavfsizligini ta'minlashda ochiq kalitli kriptotizimlarning (Asymmetric Cryptography) o'rni beqiyos. Ushbu kurs ishi doirasida diskret logarifmlash muammosiga asoslangan **ElGamal** algoritmi va uning murakkablashtirilgan turi — **Elliptik Egri Chiziqqa Asoslangan ElGamal (EC-ElGamal)** algoritmlari kompyuter dasturi ko'rinishida modellashtirildi. 

Loyiha nafaqat avtomatik shifrlash vazifasini bajaradi, balki universitetdagi "Kriptologiya" fani laboratoriya va amaliyot darslari uchun o'ziga xos vizual o'quv qo'llanma vazifasini ham o'taydi. Talabalar uni "Masala yechish" rejimida (parametrlarni qo'lda kiritish orqali) deshifrlash bosqichlarini qadamma-qadam kuzatishlari mumkin.

---

## 2. Matematik Asoslar va Algoritmlar

### 2.1. Standard ElGamal Algoritmi
Algoritm chekli maydondagi (Finite Field) murakkab matematik funksiyalarga asoslanadi.

**1. Kalitlarni generatsiya qilish:**
* Tizim katta tub son $P$ va uning generatori $G$ ni qabul qiladi.
* **Yashirin kalit (PrivateKey):** $1 < x < P-1$ oralig'idan ixtiyoriy $x$ soni tanlanadi.
* **Ochiq kalit (PublicKey):** Modulli darajaga ko'tarish orqali $h = G^x \pmod P$ hisoblanadi. Ochiq kalit $(P, G, h)$ ommaga e'lon qilinadi.

**2. Shifrlash jarayoni (Encryption):**
* Yuboriluvchi ochiq matn son ko'rinishida ($M$) olinadi. Bunda $M < P$ bo'lishi qat'iy shart.
* Tasodifiy $k$ soni ($1 < k < P-1$) tanlanadi.
* Shifr matn juftligi $(C_1, C_2)$ quyidagicha hisoblanadi:
  * $C_1 = G^k \pmod P$
  * $C_2 = M \cdot h^k \pmod P$

**3. Deshifrlash jarayoni (Decryption):**
* Qabul qilib oluvchi o'zining yashirin kaliti $x$ yordamida qadamma-qadam asl matnni tiklaydi:
  * $s = C_1^x \pmod P$ ni hisoblaydi.
  * Ochiq matn $M = C_2 \cdot s^{-1} \pmod P$ qoidasi asosida topiladi (Bu yerda $s^{-1}$ modulli teskari son bo'lib, Fermaning Kichik Teoremasi orqali hisoblanadi).

### 2.2. Elliptik Egri Chiziqli ElGamal (EC-ElGamal)
ECC (Elliptic Curve Cryptography) algoritmida qisqaroq kalit bilan yuqori darajadagi xavfsizlikka erishiladi. 

**Egri chiziq tenglamasi:** $y^2 = x^3 + Ax + B \pmod P$

**1. Kalitlarni generatsiya qilish:**
* Tizimga $P$ modul, $A, B$ koeffitsiyentlar va bazaviy nuqta $G(x, y)$ kiritiladi.
* Yashirin kalit $d$ tasodifiy tanlanadi.
* Ochiq kalit $Q(x, y) = d \cdot G$ skalyar ko'paytirish qoidasi bilan hisoblanadi.

**2. Shifrlash (Encryption):**
* Matn nuqtasi $M(x, y)$ olinadi. Tasodifiy $k$ tanlanadi.
* $C_1 = k \cdot G$
* $C_2 = M + (k \cdot Q)$
Natijada shifr juftlik $C_1$ va $C_2$ nuqtalari hosil bo'ladi.

**3. Deshifrlash (Decryption):**
* Asl matn nuqtasini topish: $M = C_2 - (d \cdot C_1)$. Bu yerda $d \cdot C_1$ nuqtasini $C_2$ dan ayirish uning y-koordinatasini manfiy $(P - y \pmod P)$ qilib qo'shish orqali olinadi.

---

## 3. Dasturiy Arxitektura ("The Standard" Yondashuvi)

Dastur Hasan Habib (The Standard) arxitekturasiga qat'iy amal qilib ishlab chiqildi va quyidagi qatlamlarga ajratildi:

1. **Modellar Qatlami (Models):**
   * Tizimda obyektlar aniq ajratilgan. Masalan, `ElGamalKeyPair`, `ECPoint`, `ECElGamalCiphertext` kabi strukturalar o'zgaruvchilarni tashiydi.
2. **Brokerlar Qatlami (Brokers):**
   * Tashqi tizimlar va fundamental mantiqlar izolyatsiya qilingan. 
   * `MathBroker`: `BigInteger` bilan modulli darajaga ko'tarish, teskari element topish kabi bazaviy operatsiyalar bajariladi.
   * `ECMathBroker`: Faqat Elliptik egri chiziq nuqtalarini qoshish (`AddPoints`) va skalyar ko'paytirish (`MultiplyPoint`) uchun xizmat qiladi.
3. **Mantiqiy Qatlam (Services):**
   * Shifrlash va deshifrlash biznes-mantiqlari `ElGamalService` va `ECElGamalService` ichida amalga oshiriladi. Ular faqat o'ziga taalluqli brokerlar bilan DI (Dependency Injection) orqali bog'langan.
4. **Vizuallashtirish Qatlami (UI):**
   * Blazor WebAssembly arxitekturasi serverga bog'lanmagan (Serverless) holatda bevosita brauzerda ulkan tezlikda matematik hisoblashlarni amalga oshiradi. 
   * Interfeys uchun MudBlazor "Antigravity" ko'rinishi qo'llanildi.

---

## 4. Dasturdan Foydalanish Qo'llanmasi

**1. Matnni Songa aylantirib shifrlash:**
Dasturga standart matnni (masalan, "Salom") shifrlash imkoniyati kiritilgan. UTF-8 orqali matn baytlar ketma-ketligi yordamida katta songa (`BigInteger`) o'giriladi. Bu funksiya orqali foydalanuvchi "M albatta P dan kichik bo'lishi kerakligi" haqidagi matematik cheklovni aniq anglab yetadi.

**2. Masala yechish va parametrlarni qo'lda kiritish:**
"Kriptologiya" fani masalalarini yechish uchun **3-Tab (Deshifrlash)** to'liq ochiq va moslashuvchan qilingan. Talaba har qanday tashqaridan berilgan $P, x, C_1$ va $C_2$ (yoki EC uchun nuqtalar) qiymatlarini kiritib, deshifrlash tugmasini bosishi va aniq javobni oraliq tushuntirish qadamlari (formulalar o'rniga qo'yilgan holati) bilan olishi mumkin.

## 5. Xulosa
Mazkur kurs ishi doirasida, zamonaviy axborot xavfsizligining eng ishonchli va murakkab algoritmlari bo'lgan ElGamal va EC-ElGamal interaktiv ravishda kompyuterda jonlantirildi. C# hamda Blazor WebAssembly texnologiyalaridan foydalanish og'ir matematik amallarni brauzerning o'zida bir zumda, xavfsiz hisoblash imkonini berdi. Dastur kelajakda kriptologiyani o'qitish va elektron hisoblash stendlari uchun mukammal poydevor hisoblanadi.
