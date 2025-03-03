# 🎮 **PANACEA**
<img width="700" alt="TitleImage" src="https://github.com/user-attachments/assets/dd0e0b72-e986-4c1d-a99d-f8405f477f41" />

## 📝 **概要 (Overview)**

**PANACEA** は、千葉大学のゲーム開発サークル **千葉大学電子計算機研究会（CCS）** から2025年2月のゲーム制作会で、約1か月間にわたり4人で開発したゲームプロジェクトです。

本作は **「親も子どもに遊ばせたい学習ゲーム」** をテーマにした 2D シューティングディフェンスゲームです。プレイヤーはナノロボットの操縦士となり、傷口から侵入する病原菌に適切な **抗生物質** を選択し、撃退していきます。

- **開発期間**: 2024年2月1日 ～ 2024年3月2日
- **チーム人数**: 4名

## 🛠️ **技術スタック (Tech Stack)**

- **開発言語**: C#
- **開発環境**: Unity 2022.3.47f1
- **バージョン管理**: GitHub

## ✨ **特徴 (Features)**

- 🦠 **多彩な戦略要素**: 10種類の病原菌と10種類の抗生物質が、効果の良・普・悪の3段階で設定されており、単純な組み合わせではなく複雑な相互作用が可能。
- 🧬 **耐性システム**: 病原菌を確実に排除できないと、使用した抗生物質の作用原理に耐性を持つ強化版の病原菌が出現。
- 🔍 **情報システムの充実**: プレイヤーが病原菌の種類や対処方法を確認できる情報システムを搭載。
- 🎨 **直感的な UI 設計**: ゲームプレイ中に情報や操作方法を自然に理解できるよう、多様な UI エフェクトを活用。

## 🖼️ **スクリーンショット (Screenshots_Add_Later)**
<img width="700" alt="InGameImage1" src="https://github.com/user-attachments/assets/85b46029-b36d-4244-8df0-d64f5ab4cdaf" />
<img width="700" alt="InGameImage2" src="https://github.com/user-attachments/assets/0c44444d-fb7b-49ff-8853-3f0b45c5a09a" />
<img width="700" alt="InGameImage3" src="https://github.com/user-attachments/assets/a5d8034e-26de-4222-ae06-131d2ff8a32e" />

## 🏗️ **担当部分及び考慮した点**

🔹 **役割**: リードプログラマー (シングルトンパターンを活用したプログラム全般の共通システム開発 (UI, サウンド, シーンロード, ゲームシステムなど), FSM パターンを活用した敵 (病原菌) AI, Unity に不慣れなメンバーのサポート)

🔹 **課題と解決**:
- **UnityとC#に慣れていないチームメンバーへの対応**: Scriptable Object (SO) を活用したデータ連携ロジックを実装し、Unity に不慣れなメンバーでも Unity Editor を通じてデータを直接編集できるようにした。
- **限られた時間と人員での開発**: `GeneralEnemy` のような共通クラスを作成し、継承とオーバーライドを活用することで 10 種類の敵ロジックを迅速に実装。また、単一のスプライトを Unity Editor 上で色調整することで、多様なグラフィック要素を作成可能なツールを開発。共通システムを API のように利用できる構造にし、各機能が効率よく連携できるよう設計。
- **大量のオブジェクトによる処理負荷の増加**: オブジェクトプーリングを導入し、オブジェクトの生成と削除を繰り返すのではなく、必要に応じてアクティブ化・非アクティブ化を行うことで最適化。

## 🚀 **プレイ方法 (How to Play)**

0. （後でビルドのリンクを上げます。）
1. **リポジトリをクローン**
   ```bash
   git clone https://github.com/AcuaRi/CCS_2025_Team2.git
   ```
2. **Unity でプロジェクトを開く**
3. **実行 (Play)**

## 📩 **お問い合わせ (Contact)**

- **GitHub**: [AcuaRi](https://github.com/AcuaRi/CCS_2025_Team2)
- **ポートフォリオサイト**: [my-portfolio](https://my-portfolio_add_later.com)
- **Email**: [your-email@example.com](acuarium2307@gmail.com)

⭐ **プロジェクトが気に入ったら、ぜひスターをお願いします！** ⭐

