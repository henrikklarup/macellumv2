using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Macellum_Remake.Models
{
    public class DataClass
    {
        readonly DatabaseRepo _repo = new DatabaseRepo();
        public void dataLoad()
        {
            const string thyborøn = @"http://www.fiskerforum.dk/auktionspriser/default.asp?auId=8026";
            const string hvidesande = @"http://www.fiskerforum.dk/auktionspriser/default.asp?auId=8016";
            const string thorsminde = @"http://www.fiskerforum.dk/auktionspriser/default.asp?auId=8126";
            const string hanstholm =
                @"http://www.fiskeauktion.dk/prices.php?auktion=1&dato=&art=";
            const string hirtshals =
                @"http://www.fiskeauktion.dk/prices.php?auktion=2&dato=&art=";
            const string strandby =
                @"http://www.fiskeauktion.dk/prices.php?auktion=5&dato=&art=";

            var allFish = _repo.GetContent(thyborøn, _repo.GetHavnId("Thyborøn")).ToList();
            allFish.AddRange(_repo.GetContent(hvidesande, _repo.GetHavnId("Hvide Sande")).ToList());
            allFish.AddRange(_repo.GetContent(thorsminde, _repo.GetHavnId("Thorsminde")).ToList());
            allFish.AddRange(_repo.GetContentFiskeContent(hanstholm, _repo.GetHavnId("Hanstholm")).ToList());
            allFish.AddRange(_repo.GetContentFiskeContent(hirtshals, _repo.GetHavnId("Hirtshals")).ToList());
            allFish.AddRange(_repo.GetContentFiskeContent(strandby, _repo.GetHavnId("Strandby")).ToList());

            _repo.AddFisk(allFish);
        }
    }
}