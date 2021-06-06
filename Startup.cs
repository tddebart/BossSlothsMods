using System.IO;
using System.Linq;
using ActualRoundsMod.Cards;
using BepInEx;
using UnboundLib.Cards;
using UnityEngine;


namespace ActualRoundsMod
{
    [BepInPlugin("MyCoolPlugin.gsgsgs.Yes", "ActualRoundsMod", "1.0.0")]
    public class Startup : BaseUnityPlugin
    {
        public static Startup Instance;
        public AssetBundle Asset;
        
        #if DEBUG
        Vector2 scrollposision;
        readonly GUILayoutOption[] gl = new GUILayoutOption[0];
        private bool _isinstanceNotNull;

        private void OnGUI()
        {
            var area = new Rect(50, 50, 2000, 2000);
            GUILayout.BeginArea(area);
            scrollposision = GUILayout.BeginScrollView(scrollposision, GUILayout.MaxWidth(200), GUILayout.MaxHeight(1000));

            if(_isinstanceNotNull && CardChoice.instance.cards != null)
            {
                GUILayout.Label("cards: ", gl);
                var cardArray = CardChoice.instance.cards;
                foreach (var info in cardArray)
                {
                    GUILayout.Label(info.cardName);
                }

            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        #endif
        
        public void Start()
        {
            Instance = this;

            #if DEBUG
            _isinstanceNotNull = CardChoice.instance != null;
            #endif
            
            Asset = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "actualart"));
            
            CustomCard.BuildCard<Sneeze>();
            CustomCard.BuildCard<YinYang>();
            CustomCard.BuildCard<InfJump>();
            CustomCard.BuildCard<Yin>();
            CustomCard.BuildCard<Yang>();
            CustomCard.BuildCard<Knockback>();

  
            //CustomCard.BuildCard<OneShot>();
            //CustomCard.BuildCard<Nice>();
            //await Task.Delay(2000);
            var cardArray = CardChoice.instance.cards;
            foreach (var info in cardArray)
            {
                switch (info.cardName)
                {
                    case "PHOENIX":
                    {
                        var infoList = info.cardStats.ToList();
                        var damage = new CardInfoStat {stat = "Damage", amount = "-50%", positive = false};
                        infoList.Add(damage);
                        infoList[0].amount = "-50%";
                        info.cardStats = infoList.ToArray();
                    
                        var gun = info.GetComponent<Gun>();
                        gun.damage = 0.5f;
                        var charstat = info.GetComponent<CharacterStatModifiers>();
                        charstat.health = 0.5f;
                        break;
                    }
                }
            }
        }

        public void Message(string message)
        {
            Logger.LogWarning(message);
        }


    }
}