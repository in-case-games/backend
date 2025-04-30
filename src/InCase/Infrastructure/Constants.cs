using Infrastructure.Models;

namespace Infrastructure;
public static class Constants {
	public static class SeedData {
		public static class GroupLootBoxes {
			public readonly static GroupLootBox Group1 = new() {
				Id = new Guid("9CE49C11-F36D-4FCE-B741-DC2BDE21769B"),
				Name = "grouplootbox1"
			};
			public readonly static GroupLootBox Group2 = new() {
				Id = new Guid("B7EB6292-26AE-4AC3-B10F-B7FE45155D1D"),
				Name = "grouplootbox2"
			};
			public readonly static GroupLootBox Group3 = new() {
				Id = new Guid("A97A6E2B-D6E9-4DAF-BE01-73CE04E737F6"),
				Name = "grouplootbox3"
			};
			
			public readonly static List<GroupLootBox> All = [
				Group1, Group2, Group3
			];
		}
		public static class LootBoxGroups {
			public readonly static LootBoxGroup Group1Box1 = new() {
				Id = new Guid("1A78EB34-4119-4B07-A874-D914E8C63741"),
				BoxId = LootBoxes.Box1.Id,
				GroupId = GroupLootBoxes.Group1.Id,
				GameId = LootBoxes.Box1.GameId
			};
			public readonly static LootBoxGroup Group2Box2 = new() {
				Id = new Guid("6C7E3D64-D778-4A65-A92A-39A089BE9F23"),
				BoxId = LootBoxes.Box2.Id,
				GroupId = GroupLootBoxes.Group2.Id,
				GameId = LootBoxes.Box2.GameId
			};
			public readonly static LootBoxGroup Group3Box3 = new() {
				Id = new Guid("0F235D21-E5CD-4CF2-A173-12163BB1961C"),
				BoxId = LootBoxes.Box3.Id,
				GroupId = GroupLootBoxes.Group3.Id,
				GameId = LootBoxes.Box3.GameId
			};

			public readonly static List<LootBoxGroup> Group1 = [
				Group1Box1
			];
			public readonly static List<LootBoxGroup> Group2 = [
				Group2Box2
			];
			public readonly static List<LootBoxGroup> Group3 = [
				Group3Box3
			];

			public readonly static List<List<LootBoxGroup>> All = [
				Group1,
				Group2,
				Group3
			];
		}
		public static class GameMarkets {
			public readonly static GameMarket CsGoTm = new() {
				Id = new Guid("F59E9641-D0AC-4B5F-A08A-726B4A548364"),
				Name = "tm",
				GameId = Games.CsGo.Id
			};
			public readonly static GameMarket Dota2 = new() {
				Id = new Guid("ECE5A73A-97EB-4F03-B28E-0D92245B2E73"),
				Name = "tm",
				GameId = Games.Dota2.Id
			};
			public readonly static List<GameMarket> All = [
				CsGoTm, Dota2
			];
		}
		public static class LootBoxes {
			public readonly static LootBox Box1 = new() { 
				Id = new Guid("E4A233D0-8DED-4004-9006-97DA7E94D708"),
				Name = "1",
				Cost = 0,
				GameId = Games.CsGo.Id,
			};
			public readonly static LootBox Box2 = new() { 
				Id = new Guid("34888621-A4ED-4E96-A893-4C60E0F95DFF"),
				Name = "2",
				Cost = 0,
				GameId = Games.CsGo.Id
			};
			public readonly static LootBox Box3 = new() { 
				Id = new Guid("951D3105-D1F6-40C3-A9AA-F507DC961A37"),
				Name = "3",
				Cost = 0,
				GameId = Games.CsGo.Id
			};
			public readonly static LootBox Box4 = new() { 
				Id = new Guid("5263253E-A08E-48CA-8723-882518384F05"),
				Name = "4",
				Cost = 0,
				GameId = Games.CsGo.Id
			};
			public readonly static LootBox Box5 = new() { 
				Id = new Guid("F76B10C4-1656-45E8-A3F4-11568BC84A3C"),
				Name = "5",
				Cost = 0,
				GameId = Games.CsGo.Id
			};

			public readonly static List<LootBox> All = [
				Box1, Box2, Box3, Box4, Box5
			];
		}
		
		public static class GameItems {
			public readonly static GameItem Ak47NightWishFn = new() {
				Id = new Guid("D67F448D-E234-4AA8-BD0F-E0972B24982C"),
				Name = "AK-47 | Nightwish (Factory-New)",
				HashName = "AK-47%20%7C%20Nightwish%20(Factory%20New)",
				IdForMarket = "4726168367-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Ak47HeadShotFn = new() {
				Id = new Guid("AB29C4FA-BA9C-4750-A953-7925E207CB6D"),
				Name = "AK-47 | Head Shot (Factory-New)",
				HashName = "AK-47%20%7C%20Head%20Shot%20(Factory%20New)",
				IdForMarket = "5932307388-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Glock18WaterElementalFn = new() {
				Id = new Guid("386F7537-346E-4898-901B-BE8C73D88597"),
				Name = "Glock-18 | Water Elemental (Factory-New)",
				HashName = "Glock-18%20%7C%20Water%20Elemental%20(Factory%20New)",
				IdForMarket = "5994330717-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem UspSCortexFn = new() {
				Id = new Guid("B2BD51CF-6E97-4627-96A8-AAF05DE798A9"),
				Name = "USP-S | Cortex (Factory-New)",
				HashName = "USP-S%20%7C%20Cortex%20(Factory%20New)",
				IdForMarket = "2735544081-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem Ssg08ParallaxFn = new() {
				Id = new Guid("DCF87BFB-D8A2-47C3-B1B7-657CFD40456B"),
				Name = "SSG 08 | Parallax (Factory-New)",
				HashName = "SSG%2008%20%7C%20Parallax%20(Factory%20New)",
				IdForMarket = "4141803976-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Rifle.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Violet.Id
			};
			public readonly static GameItem AugChameleonFn = new() {
				Id = new Guid("2D34A6D5-BF40-4A69-80CB-E4C777D77D61"),
				Name = "AUG | Chameleon (Factory-New)",
				HashName = "AUG%20%7C%20Chameleon%20(Factory%20New)",
				IdForMarket = "360525578-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Rifle.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem SawedOffTheKrakenFn = new() {
				Id = new Guid("3ECD520D-E87C-4863-B38F-8143BD263A49"),
				Name = "Sawed-Off | The Kraken (Factory-New)",
				HashName = "Sawed-Off%20%7C%20The%20Kraken%20(Factory%20New)",
				IdForMarket = "310779097-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Glock18WastelandRebelFn = new() {
				Id = new Guid("E4F5CF20-7C6A-4F81-AFAC-4B20FED3B176"),
				Name = "Glock-18 | Wasteland Rebel (Factory-New)",
				HashName = "Glock-18%20%7C%20Wasteland%20Rebel%20(Factory%20New)",
				IdForMarket = "1814948596-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem UspSJawbreakerFn = new() {
				Id = new Guid("1C2D55F7-3371-46BE-8F19-3D8A6F6F95EE"),
				Name = "USP-S | Jawbreaker (Factory New)",
				HashName = "USP-S%20%7C%20Jawbreaker%20%28Factory%20New%29",
				IdForMarket = "5721075145-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Ump45PrimalSaberFn = new() {
				Id = new Guid("A18651CA-C7D2-4837-B7D8-5E8DD24CFAFC"),
				Name = "UMP-45 | Primal Saber (Factory New)",
				HashName = "UMP-45%20%7C%20Primal%20Saber%20(Factory%20New)",
				IdForMarket = "1704421502-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem DesertEagleMechaIndustriesFn = new() {
				Id = new Guid("9BD567D4-02DA-4D21-A262-4D3CDACFC28B"),
				Name = "Desert Eagle | Mecha Industries (Factory New)",
				HashName = "Desert%20Eagle%20%7C%20Mecha%20Industries%20(Factory%20New)",
				IdForMarket = "3113374471-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem Ak47PointDisarrayFn = new() {
				Id = new Guid("37F7F8FC-2762-40D1-8F95-7BD04B6146C0"),
				Name = "AK-47 | Point Disarray (Factory New)",
				HashName = "AK-47%20%7C%20Point%20Disarray%20(Factory%20New)",
				IdForMarket = "1440531185-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem Ak47IceCoaledFn = new() {
				Id = new Guid("80002A24-D963-4934-8F76-5B1B3548566C"),
				Name = "AK-47 | Ice Coaled (Factory New)",
				HashName = "AK-47%20%7C%20Ice%20Coaled%20(Factory%20New)",
				IdForMarket = "4910470214-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem M4a4TemukauFn = new() {
				Id = new Guid("DF64F07B-FC15-405B-B3CC-685912E68B7D"),
				Name = "M4A4 | Temukau (Factory New)",
				HashName = "M4A4%20%7C%20Temukau%20(Factory%20New)",
				IdForMarket = "5199671430-5862458365",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem M4a4NeoNoirFn = new() {
				Id = new Guid("2C387DB5-311C-4F96-8C5A-285935A7DDB6"),
				Name = "M4A4 | Neo-Noir (Factory New)",
				HashName = "M4A4%20%7C%20Neo-Noir%20(Factory%20New)",
				IdForMarket = "2735407114-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Ak47SafetyNetFn = new() {
				Id = new Guid("CC0435FE-5785-4419-94A0-177BC9D56EA5"),
				Name = "AK-47 | Safety Net (Factory New)",
				HashName = "AK-47%20%7C%20Safety%20Net%20(Factory%20New)",
				IdForMarket = "3035905303-302028390",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Violet.Id
			};
			public readonly static GameItem ZeusX27OlympusFn = new() {
				Id = new Guid("33232754-3A56-438C-9048-566C3083780F"),
				Name = "Zeus x27 | Olympus (Factory New)",
				HashName = "Zeus%20x27%20%7C%20Olympus%20(Factory%20New)",
				IdForMarket = "5721066361-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem Ak47LegionOfAnubisFn = new() {
				Id = new Guid("2A47EE73-8D13-4631-92F4-D1E033EAB733"),
				Name = "AK-47 | Legion of Anubis (Factory New)",
				HashName = "AK-47%20%7C%20Legion%20of%20Anubis%20(Factory%20New)",
				IdForMarket = "3955334989-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem AwpChromaticAberrationFn = new() {
				Id = new Guid("A41D2555-21D7-4636-B83E-8B2DD12DB7D1"),
				Name = "AWP | Chromatic Aberration (Factory New)",
				HashName = "AWP%20%7C%20Chromatic%20Aberration%20(Factory%20New)",
				IdForMarket = "4910471701-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Rifle.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem M4a1SBlackLotusFn = new() {
				Id = new Guid("8E4281CB-8558-4AA5-9053-0D101A36CAD6"),
				Name = "M4A1-S | Black Lotus (Factory New)",
				HashName = "M4A1-S%20%7C%20Black%20Lotus%20(Factory%20New)",
				IdForMarket = "6002595222-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem M4a1SDecimatorFn = new() {
				Id = new Guid("4123DF64-7B2E-4539-B7F9-E6E6BCFA5270"),
				Name = "M4A1-S | Decimator (Factory New)",
				HashName = "M4A1-S%20%7C%20Decimator%20(Factory%20New)",
				IdForMarket = "6007295146-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem Mac10NeonRiderFt = new() {
				Id = new Guid("53B92D92-061A-4226-9AE6-F4E3E86F7D8E"),
				Name = "MAC-10 | Neon Rider (Field-Tested)",
				HashName = "MAC-10%20%7C%20Neon%20Rider%20(Field-Tested)",
				IdForMarket = "937290898-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem FamasZxSpectronFn = new() {
				Id = new Guid("FFEF3BFB-9F0C-4AF4-986D-5B242C708AFC"),
				Name = "FAMAS | ZX Spectron (Factory New)",
				HashName = "FAMAS%20%7C%20ZX%20Spectron%20%28Factory%20New%29",
				IdForMarket = "4578725143-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Violet.Id
			};
			public readonly static GameItem UspSTheTraitorFn = new() {
				Id = new Guid("CEE33B25-8BD7-4E36-85BD-12006F848DBD"),
				Name = "USP-S | The Traitor (Factory New)",
				HashName = "USP-S%20%7C%20The%20Traitor%20(Factory%20New)",
				IdForMarket = "5971005877-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Glock18BulletQueenFn = new() {
				Id = new Guid("6E9FD5FA-A6D9-4D2B-85A1-B6BCA2AF5058"),
				Name = "Glock-18 | Bullet Queen (Factory New)",
				HashName = "Glock-18%20%7C%20Bullet%20Queen%20(Factory%20New)",
				IdForMarket = "6007317331-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Mp7FadeFn = new() {
				Id = new Guid("DD62A0E0-5173-4B23-9B76-FBA02CAE30DB"),
				Name = "MP7 | Fade (Factory New)",
				HashName = "MP7%20%7C%20Fade%20(Factory%20New)",
				IdForMarket = "3035582416-302028390",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Violet.Id
			};
			public readonly static GameItem UspSPrintstreamFn = new() {
				Id = new Guid("2F051EDE-7255-4824-ADE4-48901D905693"),
				Name = "USP-S | Printstream (Factory New)",
				HashName = "USP-S%20%7C%20Printstream%20(Factory%20New)",
				IdForMarket = "4910473210-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem AwpPopAwpFn = new() {
				Id = new Guid("010CB5F5-070A-478D-9981-C275BBDC2221"),
				Name = "AWP | POP AWP (Factory New)",
				HashName = "AWP%20%7C%20POP%20AWP%20(Factory%20New)",
				IdForMarket = "4578725633-480085569",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Rifle.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Violet.Id
			};
			public readonly static GameItem DesertEagleConspiracyFn = new() {
				Id = new Guid("D12380BF-1E5C-4A85-9944-49F753079032"),
				Name = "Desert Eagle | Conspiracy (Factory New)",
				HashName = "Desert%20Eagle%20%7C%20Conspiracy%20(Factory%20New)",
				IdForMarket = "520027744-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Pistol.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};
			public readonly static GameItem M4a4InLivingColorFn = new() {
				Id = new Guid("D90E6B2C-1B09-4F98-B0AF-AC069EC64522"),
				Name = "M4A4 | In Living Color (Factory New)",
				HashName = "M4A4%20%7C%20In%20Living%20Color%20(Factory%20New)",
				IdForMarket = "4428772449-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Red.Id
			};
			public readonly static GameItem Ak47PhantomDisruptorFn = new() {
				Id = new Guid("89FE4D4F-A13B-40A0-86B0-4BF2904B17B2"),
				Name = "AK-47 | Phantom Disruptor (Factory New)",
				HashName = "AK-47%20%7C%20Phantom%20Disruptor%20(Factory%20New)",
				IdForMarket = "3770685021-188530139",
				Cost = 0M,
				GameId = Games.CsGo.Id,
				TypeId = GameItemTypes.Weapon.Id,
				QualityId = GameItemQualities.FactoryNew.Id,
				RarityId = GameItemRarities.Pink.Id
			};

			public readonly static List<GameItem> All = [
				Ak47NightWishFn, Ak47HeadShotFn, Glock18WaterElementalFn,
				UspSCortexFn, Ssg08ParallaxFn, AugChameleonFn,
				SawedOffTheKrakenFn, Glock18WastelandRebelFn, UspSJawbreakerFn,
				Ump45PrimalSaberFn, DesertEagleMechaIndustriesFn, Ak47PointDisarrayFn,
				Ak47IceCoaledFn, M4a4TemukauFn, M4a4NeoNoirFn,
				Ak47SafetyNetFn, ZeusX27OlympusFn, Ak47LegionOfAnubisFn,
				AwpChromaticAberrationFn, M4a1SBlackLotusFn, M4a1SDecimatorFn,
				Mac10NeonRiderFt, FamasZxSpectronFn, UspSTheTraitorFn,
				Glock18BulletQueenFn, Mp7FadeFn, UspSPrintstreamFn,
				AwpPopAwpFn, DesertEagleConspiracyFn, M4a4InLivingColorFn,
				Ak47PhantomDisruptorFn
			];
		}

		public static class LootBoxesInventories {
			public readonly static LootBoxInventory Box1Item1 = new() {
				Id = new Guid("9A6A14F5-346F-4C08-92EC-55C7F4D67610"),
				ChanceWining = 100,
				Box = LootBoxes.Box1,
				Item = GameItems.Ak47NightWishFn
			};
			public readonly static LootBoxInventory Box2Item1 = new() {
				Id = new Guid("E94134AF-2753-45D8-B5D3-765046564ED0"),
				ChanceWining = 100,
				Box = LootBoxes.Box2,
				Item = GameItems.Ak47HeadShotFn
			};
			public readonly static LootBoxInventory Box3Item1 = new() {
				Id = new Guid("52E736C7-B774-40A3-B1FD-2ABB8993BEB7"),
				ChanceWining = 100,
				Box = LootBoxes.Box3,
				Item = GameItems.Glock18WaterElementalFn
			};
			public readonly static LootBoxInventory Box4Item1 = new() {
				Id = new Guid("B96B4E6C-870D-4C72-A1B8-D7E4B0EF9DD6"),
				ChanceWining = 100,
				Box = LootBoxes.Box4,
				Item = GameItems.UspSCortexFn
			};
			public readonly static LootBoxInventory Box5Item1 = new() {
				Id = new Guid("FDAEBA3D-0983-488D-93D1-4118BC199224"),
				ChanceWining = 100,
				Box = LootBoxes.Box5,
				Item = GameItems.Ssg08ParallaxFn
			};

			public readonly static List<LootBoxInventory> Inventory1 = [
				Box1Item1,
			];
			public readonly static List<LootBoxInventory> Inventory2 = [
				Box2Item1,
			];
			public readonly static List<LootBoxInventory> Inventory3 = [
				Box3Item1,
			];
			public readonly static List<LootBoxInventory> Inventory4 = [
				Box4Item1,
			];
			public readonly static List<LootBoxInventory> Inventory5 = [
				Box5Item1,
			];
		
			public readonly static List<List<LootBoxInventory>> All = [
				Inventory1, Inventory2, Inventory3, Inventory4, Inventory5,
			];
		}

		public static class Games {
			public readonly static Game CsGo = new() {
				Id = new Guid("0FCAE961-9216-436D-AECF-131A659D8CA1"),
				Name = "csgo"
			};
			public readonly static Game Dota2 = new() {
				Id = new Guid("D26EBBF0-2EFB-46B3-8C91-A5E42140C276"),
				Name = "dota2"
			};
			
			public readonly static List<Game> All = [
				CsGo, Dota2
			];
		}

		public static class GameItemQualities {
			public readonly static GameItemQuality None = new() {
				Id = new Guid("9A351D04-4A33-409F-936E-A3210623BDBB"),
				Name = "none"
			};
			public readonly static GameItemQuality BattleScarred = new() {
				Id = new Guid("5F5A4EC8-2B10-4864-8A6E-095DBD5E7BA9"),
				Name = "battle scarred"
			};
			public readonly static GameItemQuality WellWorn = new() {
				Id = new Guid("454AA1AF-2C9A-43AA-B256-B29E0FCD3A3F"),
				Name = "well worn"
			};
			public readonly static GameItemQuality FieldTested = new() {
				Id = new Guid("8D36AE9E-6F43-4ABD-8CBC-BE5737C59AC6"),
				Name = "field tested"
			};
			public readonly static GameItemQuality MinimalWear = new() {
				Id = new Guid("2BF5DB6F-D725-408E-AC06-C66398CC60BE"),
				Name = "minimal wear"
			};
			public readonly static GameItemQuality FactoryNew = new() {
				Id = new Guid("8EA7A5D1-C9DA-4D2D-80E3-8C0880A9F5C1"),
				Name = "factory new"
			};
			
			public readonly static List<GameItemQuality> All = [
				None, BattleScarred, WellWorn, FieldTested, MinimalWear, FactoryNew
			];
		}

		public static class GameItemRarities {
			public readonly static GameItemRarity White = new() {
				Id = new Guid("5A689D96-79B9-4E2B-A675-46F29AA881C9"),
				Name = "white"
			};
			public readonly static GameItemRarity Blue = new() {
				Id = new Guid("AE604BC6-5B2F-4E15-B72F-ED66087AE1D1"),
				Name = "blue"
			};
			public readonly static GameItemRarity Violet = new() {
				Id = new Guid("E73DF5A3-9223-4194-BE4A-6D8C93E41F1A"),
				Name = "violet"
			};
			public readonly static GameItemRarity Pink = new() {
				Id = new Guid("3D61C7E5-8E89-45A8-9FEC-1BCED9B400B5"),
				Name = "pink"
			};
			public readonly static GameItemRarity Red = new() {
				Id = new Guid("45EF3969-6A1F-4F55-8CBF-D12B38CDB68A"),
				Name = "red"
			};
			public readonly static GameItemRarity Gold = new() {
				Id = new Guid("FB6D91F0-ACBB-42AE-AEE6-DFFB61479DEF"),
				Name = "gold"
			};
			
			public readonly static List<GameItemRarity> All = [
				White, Blue, Violet, Pink, Red, Gold
			];
		}

		public static class GameItemTypes {
			public readonly static GameItemType None = new() {
				Id = new Guid("CBB1C683-F1C6-46A2-A3E5-3497675465B9"),
				Name = "none"
			};
			public readonly static GameItemType Pistol = new() {
				Id = new Guid("3E59250B-4739-4E41-8B4A-97FBA2821803"),
				Name = "pistol"
			};
			public readonly static GameItemType Weapon = new() {
				Id = new Guid("E856E6F1-DBC5-407B-980D-D32F18D7D91F"),
				Name = "weapon"
			};
			public readonly static GameItemType Rifle = new() {
				Id = new Guid("A65DBCF2-67A9-4A2B-B6D0-C6F64B01C926"),
				Name = "rifle"
			};
			public readonly static GameItemType Knife = new() {
				Id = new Guid("04CF4068-23DD-475B-A031-AE5922207B4E"),
				Name = "knife"
			};
			public readonly static GameItemType Gloves = new() {
				Id = new Guid("34FE413A-6A92-4615-8D6B-0272439A6728"),
				Name = "gloves"
			};
			public readonly static GameItemType Other = new() {
				Id = new Guid("7031C467-9EA2-4B6F-ADA7-1F855D3A1F0A"),
				Name = "other"
			};
			
			public readonly static List<GameItemType> All = [
				None, Pistol, Weapon, Rifle, Knife, Gloves, Other
			];
		}
	}
}