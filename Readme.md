
# SpyEmu
Spy++で取得したログファイルから動作をシミュレートする。

Spy++ 場所

C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\spyxx.exe

C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\spyxx_amd64.exe

# 1. ログの取得
以下のようにSpy++を用いてログを表示、保存をする。
![](https://i.imgur.com/2CY3v8x.gif)

※ ログオプションにて、以下の設定がされるようにすること。
![](https://i.imgur.com/eCXqxQ8.png)

# 2. ログファイルの編集
1. にて取得したログの1行目に、「プロセス名[n番目],ウインドウクラスパスの記載を追加する。
![](https://i.imgur.com/Z4PxdU7.png)

ウインドウクラスパスは以下のような階層を「/」区切りで表現したものとする。
※ ちなみに、「Spy.FindChildWindows」メソッドで取得したウインドウクラス一覧情報にも
ウインドウクラスパス一覧が入っている。
![](https://i.imgur.com/PlpoMkB.png)

プロセス名はタスクマネージャで表示されるプロセス名から「.exe」を抜いたものとする。
また、「n番目」はタスクマネージャで表示される上からn番目のプロセスのnを指定する。

# 3. ログのシミュレート
2. にて編集したログをSpyEmuの引数に指定することで動作をシミュレートすることができる。
![](https://i.imgur.com/hMCaykp.gif)

# WindowView
Spy++ログの1行目に指定するウインドウクラスパス一覧(全プロセス)を取得するソフト。
Spy++でログを出した後にこのソフトでウインドウクラスパスを探して、ログファイルの1行目に記載する用で使う。
![](https://i.imgur.com/lRSqzmC.gif)
