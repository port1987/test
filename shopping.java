import java.util.Scanner;
import java.util.ArrayList;

/*------------------------------------*/
/*　買い物かご　クラス                   */
/*------------------------------------*/
class Basket{
  private String product;
  private int price;
  private int number;

  // --- 初期値を設定する ---
  Basket(){
    this.product = null;
    this.price = 0;
    this.number = 0;
  }
  // --- フィールド変数全てにデータをセットする ---
  Basket(String product, int price, int number){
    this.product = product;
    this.price = price;
    this.number = number;
  }
  // --- アクセッサメソッド ---
  // --- 商品名　ゲッタ ---
  public String getProduct{
    return this.product;
  }
  // --- 商品名　セッタ ---
  public void setProduct(String product){
    this.product = product;
  }
  // --- 価格　ゲッタ ---
  public int getPrice{
    return this.price;
  }
  // --- 価格　セッタ ---
  public void setPrice(int price){
    this.price = price;
  }
  // --- 個数　ゲッタ ---
  public int getNumber{
    return this.number;
  }
  // --- 個数　セッタ ---
  public void setNumber(int number){
    this.number = number;
  }
}

/*------------------------------------*/
/*　買い物　クラス　　                   */
/*------------------------------------*/
public class Shopping{
  public static void main(String[] args){
    Scanner scanner = new Scanner(System.in); //Scannerクラスのインスタンスを生成（標準入力を指定）
    ArrayList<Basket> basket = new ArrayList<Basket>(); //買い物かごのインスタンスを生成

    // --- 商品登録 ---
    System.out.println("*** 買い物かごに商品を登録します ***");

    for (int i = 0; i < 5; i++) {

      System.out.print("商品名 ==>");
      String pProduct = scanner.nextLine(); //商品名を取得

      System.out.print("価　格 ==>");
      int pPrice = Integer.parseInt(scanner.nextLine()); //価格を取得

      System.out.print("個　数 ==>");
      int pNumber = Integer.parseInt(scanner.nextLine()); //個数を取得
      System.out.println();

      basket.add(new Basket(pProduct, pPrice, pNumber));
    }
    // --- 商品の表示 ---
    System.out.println("*** 買い物かごの商品を表示します ***");

    for (int i = 0; i < basket.size(); i++){
      System.out.println("--- 買い物かごの商品：" + (i+1) + "品目 ---");
      System.out.println("商品名：" + basket.get(i).getProduct());
      System.out.println("価　格：" + basket.get(i).getPrice());
      System.out.println("個　数：" + basket.get(i).getNumber());
      System.out.println();
    }
  }
}
