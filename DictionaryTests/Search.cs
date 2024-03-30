using RusEngDictionary;

namespace DictionaryTests
{
    [TestClass]
    public class Search
    {
        [TestMethod]
        public void SelectedIsNotNull()
        {
            string _pattern = "";

            DicionaryViewModel dVModel = new DicionaryViewModel();

            dVModel.Set(ref _pattern, "Ball");
            dVModel.Pattern = _pattern;
            dVModel.Selected = dVModel.items.FirstOrDefault(s => s.Word.StartsWith(dVModel.Pattern));
            Assert.IsNotNull(dVModel.Selected);
        }
        [TestMethod]
        public void SelectedAreEqualPatternWord()
        {
            string _pattern = "";
            string _selected = "";

            DicionaryViewModel dVModel = new DicionaryViewModel();
            dVModel.Pattern = _pattern;
            dVModel.Set(ref _pattern, "Ball");

            //Сравниваем в коллекции слово из свойства Pattern если слово в коллекции есть то присваеваем значение в свойство Selected иначе возвращаем null
            dVModel.Selected = dVModel.items.FirstOrDefault(s => s.Word.StartsWith(dVModel.Pattern));
            dVModel.Set(ref _selected, dVModel.Selected.Word);

            Assert.AreEqual(_pattern, _selected);
        }
    }
}