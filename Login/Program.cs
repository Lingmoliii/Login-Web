using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// ��� Razor ҳ�����
builder.Services.AddControllersWithViews();

// ��������֤����ʹ�� JWT Bearer �� Keycloak ����
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CookieAuth"; // Ĭ��ʹ�� Cookie
    options.DefaultChallengeScheme = "oidc";          // ��սʱʹ�� OIDC
})
.AddCookie("CookieAuth", options =>
{
    options.Cookie.Name = "UserLoginCookie";
    options.LoginPath = "/Account/Login";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "http://localhost:8080/realms/myrealm"; // Keycloak ��������ַ
    options.ClientId = "myclient";                              // Keycloak Client ID
    options.ClientSecret = "FCIXFynpcqvUJPIcnu3aO4goE1obvPp4";  // Keycloak Client Secret
    options.ResponseType = "code";                              // ʹ�� Authorization Code Flow
    options.GetClaimsFromUserInfoEndpoint = true;               // ���û���Ϣ�˵��ȡ Claims
    options.SaveTokens = true;                                  // ������ʺ�ˢ������

    // ���� Token ��֤
    options.TokenValidationParameters.ValidateAudience = true;
    options.TokenValidationParameters.ValidAudience = "myclient"; // �� Keycloak Client ID ƥ��
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ValidIssuer = "http://localhost:8080/realms/myrealm";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ���������֤�м��
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
