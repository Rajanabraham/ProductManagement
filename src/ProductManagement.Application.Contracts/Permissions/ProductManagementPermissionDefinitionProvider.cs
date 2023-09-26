using ProductManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ProductManagement.Permissions;

public class ProductManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var ProductManagementGroups = context.AddGroup(ProductManagementPermissions.GroupName,L("ProductManagement"));
        var ProductsPermisions =ProductManagementGroups.AddPermission(ProductManagementPermissions.Products.Default,L("Permission:Products"));
        ProductsPermisions.AddChild(ProductManagementPermissions.Products.Create, L("Permission:Create"));
        ProductsPermisions.AddChild(ProductManagementPermissions.Products.Update, L("Permission:Update"));
        ProductsPermisions.AddChild(ProductManagementPermissions.Products.Delete, L("Permission:Delete"));


       // context.GetPermissionOrNull(ProductManagementPermissions.Products.Update).IsEnabled = false;

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProductManagementResource>(name);
    }
}
