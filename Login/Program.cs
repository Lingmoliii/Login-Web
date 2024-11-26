using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// ��� Razor ҳ��������֤����
builder.Services.AddControllersWithViews();

// ���������֤
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "http://localhost:8080/realms/myrealm"; // ʹ�� http Э��
    options.ClientId = "myclient";
    options.ClientSecret = "FCIXFynpcqvUJPIcnu3aO4goE1obvPp4";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    // ���� HTTPS Ҫ�󣬿��������п����� HTTP
    options.RequireHttpsMetadata = false;
});

var app = builder.Build();

// �����м��
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ������������ʾ�쳣ҳ��
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ���������֤�м��
app.UseAuthorization(); // ������Ȩ�м��

// ����Ĭ��·��
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ���ûص�·��
app.MapGet("/callback", (HttpContext context) =>
{
    // ��ȡ�ص��е���Ȩ��
    var code = context.Request.Query["code"];
    if (!string.IsNullOrEmpty(code))
    {
        // ������ʹ����Ȩ���ȡ Token
        return Results.Ok($"Received authorization code: {code}");
    }
    return Results.BadRequest("Authorization code missing.");
});

app.Run();
