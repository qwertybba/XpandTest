using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace XpandTest.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Settings")]
[XafDisplayName("Category")]
public class NormativeMonitoringCategory : AuditableBase
{ 
    public NormativeMonitoringCategory(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
    }
    private string _Title;
    [RuleUniqueValue]
    [XafDisplayName("Title"), ToolTip("Please enter the title of the normative category")]
    [RuleRequiredField(DefaultContexts.Save)]
    public string Title
    {
        get => _Title;
        set => SetPropertyValue(nameof(Title), ref _Title, value);
    }

    [Association("NormativeMonitoringCategory-NormativeMonitoringSubCategory")]
    [XafDisplayName("Sub Categories")]
    public XPCollection<NormativeMonitoringSubCategory> SubCategories => GetCollection<NormativeMonitoringSubCategory>(nameof(SubCategories));
}