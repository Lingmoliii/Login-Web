using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// ��������֤����
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.Authority = "http://localhost:8080/realms/myrealm"; // Keycloak ��ַ
    options.ClientId = "Login2"; // �µ� Client ����
    options.ClientSecret = "T2sHscEm4saufQEaBA8OfJmkA0HBQMFN"; // �µ� Client Secret
    options.ResponseType = OpenIdConnectResponseType.Code; // ʹ�� Authorization Code Flow
    options.SaveTokens = true; // ��������
    options.GetClaimsFromUserInfoEndpoint = true; // �� UserInfo ��ȡ�û���Ϣ
    options.RequireHttpsMetadata = false; // ���� HTTPS Ԫ�������ƣ�������������
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "http://localhost:8080/realms/myrealm",
        ValidateAudience = true,
        ValidAudience = "Login2",
        ValidateLifetime = true
    };
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

// �����м��
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();