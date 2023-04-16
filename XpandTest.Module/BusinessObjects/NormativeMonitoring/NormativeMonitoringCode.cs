using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace XpandTest.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Settings")]
[XafDisplayName("Code")]
public class NormativeMonitoringCode : AuditableBase
{ 
    public NormativeMonitoringCode(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
    }
    private string _Title;
    [RuleUniqueValue]
    [XafDisplayName("Code"), ToolTip("Please enter the normative code")]
    [RuleRequiredField(DefaultContexts.Save)]
    public string Title
    {
        get => _Title;
        set => SetPropertyValue(nameof(Title), ref _Title, value);
    }

   
}