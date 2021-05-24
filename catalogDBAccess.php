<?php
  /*--------------------------------------------*/
  /*　mysqliを使用してcatalog DB よりデータを読み込み*/
  /*--------------------------------------------*/
  $title = 'データベースのデータ;
  $CatalogPool = array(); //DBから読み出したデータを格納する配列
  $CatalogLine = array(); //DBの一行分データ

  //--- (1)データベースを指定して接続の確立 ---
  $db = mysqli_connect('192.168.1.1','username','password','DBname');
  if ($db === null) {
    die('接続失敗',mysqli_connect_ettor());
  }

  //--- (2)SQLの発行 ---
  ＄stmt = mysqli_query($db, 'SELECT * from catalog');

  //--- (3)行数の取得 ---
  $rs = mysqli_num_rows($stmt);

  //--- (4)データの取得 ---
  for ($i = 0; $i < $rs; i++) {
    $data = mysqli_fetch_assoc($stmt);
    $CatalogLine = array('id' => $data['id'], 'name' => $data['name'],
      'price' => $data['price'],'detail' => $data['detail'],'pictureUrl'
       => $data['pictureUrl']);
       $CatalogPool[$i] = $CatalogLine;
  }

  //--- (5)DB接続のクローズ ---
  mysqli_close($db);
?>

<!DOCTYPE html>
<html lang = "ja">
<head>
  <title><?=$title?></title>
  <meta charset = "UTF-8">
</head>
<body>
<?php
  //--- 読み込んだデータの表示 ---
  $cnt = count($CatalogPool);
  for ($i = 0; $i < $cnt; $i++) {
    echo '<p>---' . $i . '件目 ---</p>';
    echo '<p>id = ' . $CatalogPool[$i]['id'] . '</p>';
    echo '<p>price = ' . $CatalogPool[$i]['price'] . '</p>';
    echo '<p>detail = ' . $CatalogPool[$i]['detail'] . '</p>';
    echo '<p>pictureUrl = ' . $CatalogPool[$i]['pictureUrl'] . '</p>';
    '<img src =' . $CatalogPool[$i]['pictureUrl'] . '>';
  }
?>
</body>
</html>
