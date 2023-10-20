namespace ToDoBlazor.Pages;

using Microsoft.AspNetCore.Components;
using MudBlazor;

public class IndexComponent : ComponentBase
{
    public bool DisableField = true;

    public string _FieldValue = String.Empty;

    public MudBlazor.Color ButtonColor = Color.Primary;

    public string ButtonText = ButtonTextState.NewTask;

    public List<Element> Elements;

    public string _searchString = String.Empty;

    public bool _sortNameByLength;

    DialogService _DialogService = new();

    public List<string> _events = new();

    public string FieldValue
    {
        get => _FieldValue;

        set
        {
            if (_FieldValue != value)
            {
                _FieldValue = value;
                FieldIsChanging();
            }
        }
    }

    public IndexComponent()
    {
        Elements = new List<Element>() 
        {
            new Element("Cook", 1, 1),
            new Element("Clean", 2, 1),
            new Element("Dish", 3, 2),
            new Element("Sleep", 4, 2)
        };

        ButtonColor = Color.Primary;
    }

    public static class ButtonTextState
    {
        public static string NewTask = "New Task";
        public static string CancelTask = "Cancel Task";
        public static string SaveTask = "Save Task";
    }

    public record Element(string Name, int Number, int Group);

    // custom sort by name length
    public Func<Element, object> _sortBy => x =>
    {
        if (_sortNameByLength)
            return x.Name.Length;
        else
            return x.Name;
    };

    // quick filter - filter globally across multiple columns with the same input
    public Func<Element, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ($"{x.Number}".Contains(_searchString))
            return true;

        return false;
    };

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    // events
    public void RowClicked(DataGridRowClickEventArgs<Element> args)
    {
        _events.Insert(0, $"Event = RowClick, Index = {args.RowIndex}, Data = {System.Text.Json.JsonSerializer.Serialize(args.Item)}");
    }

    public void SelectedItemsChanged(HashSet<Element> items)
    {
        _events.Insert(0, $"Event = SelectedItemsChanged, Data = {System.Text.Json.JsonSerializer.Serialize(items)}");
    }

    public void ChangeButtonType()
    {
        if (ButtonColor == Color.Primary)
        {
            ButtonColor = Color.Secondary;
            ButtonText = ButtonTextState.CancelTask;
            DisableField = false;
        }
        else if (ButtonColor == Color.Secondary)
        {
            FieldValue = String.Empty;

            ButtonColor = Color.Primary;
            ButtonText = ButtonTextState.NewTask;
            DisableField = true;
        }
        else if(ButtonColor == Color.Success)
        {
            var lenght = Elements.Count();
            Elements.Add(new Element(FieldValue, lenght + 1, lenght - 2));
            FieldValue = String.Empty;

            ButtonColor = Color.Primary;
            ButtonText = ButtonTextState.NewTask;
            DisableField = true;
        }
    }

    public void FieldIsChanging()
    {
        if (FieldValue.Length > 0)
        {
            ButtonColor = Color.Success;
            ButtonText = ButtonTextState.SaveTask;
        }
        else if (FieldValue.Length == 0)
        {
            ButtonColor = Color.Secondary;
            ButtonText = ButtonTextState.CancelTask;
        }
    }
}