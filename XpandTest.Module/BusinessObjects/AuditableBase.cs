using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace XpandTest.Module.BusinessObjects;
[NonPersistent]
public abstract class AuditableBase : BaseObject
{
    public AuditableBase(Session session)
        : base(session)
    {
    }
    ApplicationUser GetCurrentUser()
    {
        return Session.FindObject<ApplicationUser>(CriteriaOperator.Parse("Oid=CurrentUserId()"));
    }
    public override void AfterConstruction()
    {
        base.AfterConstruction();
        CreatedOn = DateTime.Now;
        CreatedBy = GetCurrentUser();
    }
    protected override void OnSaving()
    {
        base.OnSaving();
        UpdatedOn = DateTime.Now;
        UpdatedBy = GetCurrentUser();
    }
    ApplicationUser createdBy;
    [ModelDefault("AllowEdit", "False")]
    [VisibleInListView(false)]
    [NonCloneable]
    public ApplicationUser CreatedBy
    {
        get => createdBy;
        set => SetPropertyValue(nameof(CreatedBy), ref createdBy, value);
    }
    DateTime createdOn;
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
    [VisibleInListView(false)]
    [NonCloneable]
    public DateTime CreatedOn
    {
        get => createdOn;
        set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
    }
    ApplicationUser updatedBy;
    [ModelDefault("AllowEdit", "False")]
    [VisibleInListView(false)]
    [NonCloneable]
    public ApplicationUser UpdatedBy
    {
        get => updatedBy;
        set => SetPropertyValue(nameof(UpdatedBy), ref updatedBy, value);
    }
    DateTime updatedOn;
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
    [VisibleInListView(false)]
    [NonCloneable]
    public DateTime UpdatedOn
    {
        get => updatedOn;
        set => SetPropertyValue(nameof(UpdatedOn), ref updatedOn, value);
    }
}