using Models = API.Models;
using Entities = API.Implementations.Repository.Entities;

namespace API.Utils;

public static class Extensions
{
    public static Models.Question ToModel(this Entities.Question question)
    {
        return new Models.Question
        {
            Id = question.Id,
            Text = question.Text,
            Category = Enum.Parse<Models.RiasecCategory>(question.Category)
        };
    }
    
    public static bool IsAny<T>(this List<T>? list)
    {
        return list != null && list.Count != 0;
    }
}