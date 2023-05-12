using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit;
using Microsoft.AspNetCore.StaticFiles;


namespace XpandTest.Module.BusinessObjects;
[DefaultClassOptions]
[XafDefaultProperty($"{nameof(Code)} {nameof(Title)}")]
[NavigationItem("DashBoard")]
[ImageName("EnableSearch")]
[Appearance("RowGray", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "ListView", Criteria = "IsApplicable==False", BackColor = "Gray", FontColor = "Black")]
[Appearance("RowYellow", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "ListView", Criteria = "IsApplicable==True And PublishedEdition > CompanyEdition And File is not null", BackColor = "Yellow", FontColor = "Black")]
//[Appearance("RowOrange", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "ListView", Criteria = "ShipmentStatus.Title=='On Rad'", BackColor = "Orange")]
[Appearance("RowRed", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "ListView", Criteria = "IsApplicable==True And File is null", BackColor = "Red", FontColor = "Black")]
[Appearance("RowGreen", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "ListView", Criteria = "IsApplicable==True And PublishedEdition == CompanyEdition And File is not null", BackColor = "Green", FontColor = "Black")]
//[Appearance("ColRed", AppearanceItemType = "ViewItem", TargetItems = "DebtsCategory;PaymentsTotal", Context = "Any", Criteria = "PaymentDeadline Is Not Null And PaymentDeadline <= ADDDAYS(Today(), 7) And DebtsCategory.Title == 'Ouverte'", BackColor = "Red", FontColor = "Black")]

public class NormativeMonitoring : AuditableBase
{
    public NormativeMonitoring(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
        IsApplicable = true;
        //object lastSeen = Session.Evaluate(typeof(Person), CriteriaOperator.Parse("Max(LastSeen)"),null);
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        //LastSeen = DateTime.Now;
        //TODO Save LastSeen automatically on close
    }
    private NormativeMonitoringCode _Code;
    [XafDisplayName("Code")]
    [RuleRequiredField(DefaultContexts.Save)]
    public NormativeMonitoringCode Code
    {
        get => _Code;
        set => SetPropertyValue(nameof(Code), ref _Code, value);
    }
    private NormativeMonitoringCategory _Category;
    [XafDisplayName("Category")]
    [RuleRequiredField(DefaultContexts.Save)]
    public NormativeMonitoringCategory Category
    {
        get => _Category;
        set => SetPropertyValue(nameof(Category), ref _Category, value);
    }
    private NormativeMonitoringSubCategory _SubCategory;
    [DataSourceProperty("Category.SubCategories", DataSourcePropertyIsNullMode.SelectNothing)]
    [XafDisplayName("Sub Category")]
    public NormativeMonitoringSubCategory SubCategory
    {
        get => _SubCategory;
        set => SetPropertyValue(nameof(SubCategory), ref _SubCategory, value);
    }

    private string _Title;
    [Size(370)]
    [RuleUniqueValue]
    [XafDisplayName("Title"), ToolTip("Please enter the normative title")]
    [RuleRequiredField(DefaultContexts.Save)]
    public string Title
    {
        get => _Title;
        set => SetPropertyValue(nameof(Title), ref _Title, value);
    }

    private DateTime _PublishedEdition;
    [XafDisplayName("Published Edition"), ToolTip("Please enter the date of the last published edition")]
    [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
    [RuleRequiredField(DefaultContexts.Save)]
    public DateTime PublishedEdition
    {
        get => _PublishedEdition;
        set => SetPropertyValue(nameof(PublishedEdition), ref _PublishedEdition, value);
    }

    private DateTime _CompanyEdition;
    [XafDisplayName("Company Edition"), ToolTip("Please enter the date of the latest edition available in your organization")]
    [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
    [RuleValueComparison("RuleComparison_PublishedEdition_CompanyEdition", DefaultContexts.Save, ValueComparisonType.LessThanOrEqual,
    "PublishedEdition", ParametersMode.Expression)]
    public DateTime CompanyEdition
    {
        get => _CompanyEdition;
        set => SetPropertyValue(nameof(CompanyEdition), ref _CompanyEdition, value);
    }
    private bool _IsApplicable;
    [XafDisplayName("Is Applicable")]
    public bool IsApplicable
    {
        get => _IsApplicable;
        set => SetPropertyValue(nameof(IsApplicable), ref _IsApplicable, value);
    }

    private FileData _File;
    [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
    [FileTypeFilter("PDFFiles", 1, "*.pdf")]
    [XafDisplayName("Document")]
    public FileData File
    {
        get => _File;
        set => SetPropertyValue(nameof(File), ref _File, value);
    }

    [VisibleInListView(false), VisibleInDetailView(true)]
    [ModelDefault("PropertyEditorType", "XpandTest.Blazor.Server.Editors.FileDataExtend.FileDataPropertyEditor")]
    [EditorAlias("FileDataPropertyEditor")]
    public string Content
    {
        get
        {
            if (File == null) return string.Empty;
            using var ms = new MemoryStream();
            File.SaveToStream(ms);
            ms.Position = 0;
            string filename = File.FileName.ToLower();
            string mimeType = GetMimeType(filename);
            string content = $"data:{mimeType};base64,";
            content += Convert.ToBase64String(ms.ToArray());
            ms.Dispose();
            return content;
        }
    }

    private static string GetMimeType(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        string contentType = provider.TryGetContentType(fileName, out contentType) ? contentType : "application/octet-stream";
        return contentType;
    }
    private string _Observation;
    [Size(SizeAttribute.Unlimited)]
    [XafDisplayName("Observations"), ToolTip("Please enter any other observations")]
    public string Observation
    {
        get { return _Observation; }
        set { SetPropertyValue(nameof(Observation), ref _Observation, value); }
    }

    //DateTime _LastSeen;
    //[ModelDefault("AllowEdit", "False")]
    //[ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
    //[VisibleInListView(false), VisibleInDetailView(false)]
    //[NonCloneable]
    //public DateTime LastSeen
    //{
    //    get => _LastSeen;
    //    set => SetPropertyValue(nameof(LastSeen), ref _LastSeen, value);
    //}
}