#ifdef __cplusplus
extern "C" {
#endif
    void CleanIconBadge(int i){
    	NSLog(@"setBadge");
        NSLog(@"%d",i);

    	[UIApplication sharedApplication].applicationIconBadgeNumber = i;
	}
#ifdef __cplusplus
}
#endif

