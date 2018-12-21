
namespace StateEvaluation.Common.Constants
{
    public static class MessageBoxConstants
    {
        public static string
            WrongFields = "Ошибка, проверьте введенные поля",
            BindFailed = "Ошибка при биндинге полей",
            ErrorUpdating = "Ой, ошибка обновления",

            TestCreated = "Преференция была создана",
            TestDeleted = "Преференция была удалена",
            TestUpdated = "Преференция была обновлена",
            ErrorTestCreate = "Ой, не удалось создать преференцию",
            ErrorTestDelete = "Ой, не удалось удалить преференцию",
            ErrorTestUpdate = "Ой, не удалось обновить преференцию",

            FeelCreated = "Субьективное ощущение было создано",
            FeelDeleted = "Субьективное ощущение было удалено",
            FeelUpdated = "Субьективное ощущение было обновлено",
            ErrorFeelCreate = "Ой, произошла ошибка при создании субьективного ощущения",
            ErrorFeelDelete = "Ой, произошла ошибка при удалении субьективного ощущения",
            ErrorFeelUpdate = "Ой, произошла ошибка при редактировании субьективного ощущения",

            PersonCreated = "Запись о человеке была создана",
            PersonDeleted = "Запись о человеке была удалена",
            PersonUpdated = "Запись о человеке была обновлена",
            ErrorPersonCreate = "Ой, ошибка создания записи о человеке",
            ErrorPersonDelete = "Ой, мы не можем удалить эту запись, так как существуют связанные с ней данные",
            ErrorPersonUserID = "Ошибка! Длина Експедиции и Номера человека не должна превышать 7 в сумме",
            ErrorPersonLastname = "Ошибка! Фамилия должна быть не больше 20 символов",
            ErrorPersonFirstname = "Ошибка! Имя должно быть не больше 20 символов",
            ErrorPersonMiddlename = "Ошибка! Отчество должно быть не больше 20 символов",
            ErrorPersonWorkposition = "Ошибка! Длина профессии должна быть не более 20 символов";
    }
}
